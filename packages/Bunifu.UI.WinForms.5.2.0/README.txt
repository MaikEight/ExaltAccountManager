Breaking changes in v2.2.7 and higher: 
-------------------------------------------------------------------------------------------
In Bunifu Dropdown, we have removed non CLS-compliant properties in Visual Basic 
which led to ambiguous properties being reported by Visual Basic developers due to 
the case insensitive nature of the language. For C# developers, this may mean you 
will receive an error stating: "BunifuDropdown does not contain a definition of 
'ItemHighlightColor'..." and "BunifuDropdown does not contain a definition of 
'ItemHighlightForeColor'...". If so, kindly go ahead and delete the specific 
lines of code with the mentioned properties from the Form's Designer file and 
you're good to go. You can then build your project successfully.

For users who have reported issues revolving around ambigous namespaces e.g. 
Bunifu TextBox, kindly go ahead and delete the reference 'Bunifu.UI.WinForms.Deprecated' 
from your project's References. This reference specifically provides backward compatibility 
with past non-NuGet releases.

For any issues, feel free to reach out via our Chat platform on our homepage.