﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UnitTesting {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("UnitTesting.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #include &lt;iostream&gt;
        ///using std::cout;
        ///
        ///int main()
        ///{
        ///    /* Here is a
        ///        //*Big
        ///        //*Comment
        ///        //*Block
        ///    */
        ///    
        ///    // Pointer literal
        ///    int* pointer_int = nullptr;
        ///    
        ///    // Boolean literals
        ///    bool is_true = true;
        ///    bool is_false = false;
        ///
        ///    // String and character literal
        ///    cout &lt;&lt; &quot;Hello World&quot; &lt;&lt;&apos;!&apos; &lt;&lt; &apos;\n&apos;;
        ///
        ///    // Invalid character literals
        ///    cout &lt;&lt; &apos;!!&apos; &lt;&lt; &apos;\\n&apos; &lt;&lt; &apos;&apos;;
        ///    
        ///    // Some integer literals
        ///    cout &lt;&lt; 123    &lt;&lt; &apos;\n&apos;
        ///         &lt;&lt;  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string lexicalAnalysisSampleSource1 {
            get {
                return ResourceManager.GetString("lexicalAnalysisSampleSource1", resourceCulture);
            }
        }
    }
}
