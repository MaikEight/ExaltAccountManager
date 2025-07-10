// Navigation service for programmatic navigation
// This allows navigation from outside React components (like system tray)

class NavigationService {
    constructor() {
        this.navigate = null;
    }

    setNavigate(navigateFunction) {
        this.navigate = navigateFunction;
    }

    navigateTo(path) {
        if (this.navigate) {
            this.navigate(path);
            return true;
        } else {
            console.warn('Navigation function not set. Make sure NavigationService is initialized.');
            return false;
        }
    }

    // Predefined navigation methods for common pages
    goToAccounts() {
        return this.navigateTo('/accounts');
    }

    goToUtilities() {
        return this.navigateTo('/utilities');
    }

    goToSettings() {
        return this.navigateTo('/settings');
    }

    goToVaultPeeker() {
        return this.navigateTo('/vaultPeeker');
    }

    goToDailyLogins() {
        return this.navigateTo('/dailyLogins');
    }

    goToProfile() {
        return this.navigateTo('/profile');
    }

    goToLogs() {
        return this.navigateTo('/logs');
    }

    goToAbout() {
        return this.navigateTo('/about');
    }

    goToFeedback() {
        return this.navigateTo('/feedback');
    }

    goToImporter() {
        return this.navigateTo('/importer');
    }
}

// Create a singleton instance
const navigationService = new NavigationService();

export default navigationService;
