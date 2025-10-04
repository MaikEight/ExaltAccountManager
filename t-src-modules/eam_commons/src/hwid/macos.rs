#[cfg(target_os = "macos")]
pub async fn get_device_unique_identifier() -> Result<String, String> {
    use core_foundation::base::{
        kCFAllocatorDefault, CFGetTypeID, CFTypeRef, TCFType,
    };
    use core_foundation::string::{CFString, CFStringRef};
    use libc::c_char;
    use std::ffi::CString;

    type kern_return_t = i32;
    type io_object_t = u32;           // mach_port_t
    type io_registry_entry_t = io_object_t;
    type io_option_t = u32;

    #[link(name = "IOKit", kind = "framework")]
    extern "C" {
        fn IOServiceMatching(name: *const c_char) -> *const std::ffi::c_void; // CFMutableDictionaryRef
        fn IOServiceGetMatchingService(master_port: u32, matching: *const std::ffi::c_void) -> io_object_t;
        fn IORegistryEntryCreateCFProperty(
            entry: io_registry_entry_t,
            key: CFStringRef,
            allocator: *const std::ffi::c_void,
            options: io_option_t,
        ) -> CFTypeRef;
        fn IOObjectRelease(obj: io_object_t) -> kern_return_t;
    }

    #[link(name = "CoreFoundation", kind = "framework")]
    extern "C" {
        fn CFRelease(cf: CFTypeRef);
    }

    unsafe {
        // Match IOPlatformExpertDevice
        let cls = CString::new("IOPlatformExpertDevice").map_err(|e| e.to_string())?;
        let matching = IOServiceMatching(cls.as_ptr());
        if matching.is_null() {
            return Err("IOServiceMatching returned null".into());
        }

        // 0 is kIOMainPortDefault
        let service = IOServiceGetMatchingService(0, matching);
        if service == 0 {
            return Err("IOServiceGetMatchingService returned 0 (not found)".into());
        }

        // Read "IOPlatformUUID" (kIOPlatformUUIDKey)
        let key = CFString::new("IOPlatformUUID");
        let value = IORegistryEntryCreateCFProperty(
            service,
            key.as_concrete_TypeRef(),
            kCFAllocatorDefault,
            0,
        );

        // Always release the service object
        IOObjectRelease(service);

        if value.is_null() {
            return Err("IORegistryEntryCreateCFProperty returned null".into());
        }

        // Ensure it's a CFString, otherwise release and error
        if CFGetTypeID(value) != CFString::type_id() {
            CFRelease(value);
            return Err("IOPlatformUUID is not a CFString".into());
        }

        // Take ownership under create rule and convert to Rust String
        let cfstr: CFString = TCFType::wrap_under_create_rule(value as CFStringRef);
        let uuid = cfstr.to_string();

        if uuid.is_empty() {
            Err("IOPlatformUUID was empty".into())
        } else {
            Ok(uuid)
        }
    }
}
