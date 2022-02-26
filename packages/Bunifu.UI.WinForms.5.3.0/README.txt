Breaking changes from v5.2.0 (only)
-------------------------------------------------------------------------------------------
In our recent v5.2.0 release (now deprecated), we noticed that Bunifu Button included two 
properties namely: 'onHoverState' and 'OnHoverState'. We had to resolve this issue through  
removing the latter property which in VisualBasic led to an ambiguous property error being 
reported by Visual Basic developers due to the case-insensitive nature of the language. 
For C# developers who installed v5.2.0 and have now upgraded to v5.2.1 or higher, 
you will receive the following error: 

  >> "BunifuButton does not contain a definition of 'OnHoverState'..."

If so, kindly go ahead and delete the specific lines of code with the mentioned property 
from the Form's Designer to resolve this. You can then build your project successfully.
Kindly note that this will not affect anyone upgrading from other previous versions 
other than the deprecated version 5.2.0 which is no longer active.

Breaking changes in v2.2.7 and higher
-------------------------------------------------------------------------------------------
In Bunifu Dropdown, we have removed non CLS-compliant properties in Visual Basic which led 
to ambiguous properties being reported by Visual Basic developers due to the case-insensitive 
nature of the language. For C# developers, this may mean you will receive an error stating: 
"BunifuDropdown does not contain a definition of 'ItemHighlightColor'", and "BunifuDropdown 
does not contain a definition of 'ItemHighlightForeColor'". If so, kindly go ahead and 
delete the specific lines of code with the mentioned properties from the Form's Designer 
and you're good to go. You can then build your project successfully.

FAQs:
-------------------------------------------------------------------------------------------
Q1: I'm getting an "ambiguous namespace" error after upgrading. What should I do?
-------------------------------------------------------------------------------------------
For users who have reported issues revolving around ambigous namespace error after upgrade 
which mostly revolve around Bunifu TextBox, kindly go ahead and delete the reference 
'Bunifu.UI.WinForms.Deprecated' from your project's References. This specific reference 
provides backward compatibility for past non-NuGet releases in v1.11.5.21 and below.

-------------------------------------------------------------------------------------------
Q2: I'm getting the error, "This license does not have a registered UI WinForms license..."
-------------------------------------------------------------------------------------------
If you face this issue, kindly go ahead and edit the licenses.licx file found in your 
project's Properties section by expanding it and locating the file. You will notice that 
the error will be pointing to a specific control/component. Navigate through the list of 
controls and components registered and ensure that the control is registered in the file. 
If not, simply go ahead and add the control/component inside the file in this format: 

  [control name], [assembly name]

  Example:
  --------
  Bunifu.UI.WinForms.BunifuToolTip, Bunifu.UI.WinForms.BunifuToolTip

Once done, save and close the file, then rebuild your project for changes to take effect.
If this doesn't work, please reach out to us via our Chat platform on our homepage.

We are always available via our Chat platform on our homepage, therefore in case you 
face any support issues, please feel free to reach out to and mention the issue. You  
will be sure to hear from us thereafter. All the best, and happy coding!

---
Bunifu Framework Team