﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Editor.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Editor.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap mspaint_title_card {
            get {
                object obj = ResourceManager.GetObject("mspaint_title_card", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // A simple Lexer meant to demonstrate a few theoretical concepts. It can
        ///// support several parser concepts and is very fast (though speed is not its
        ///// design goal).
        /////
        ///// J. Arrieta, Nabla Zero Labs
        /////
        ///// This code is released under the MIT License.
        /////
        ///// Copyright 2018 Nabla Zero Labs
        /////
        ///// Permission is hereby granted, free of charge, to any person obtaining a copy
        ///// of this software and associated documentation files(the &quot;Software&quot;), to deal
        ///// in the Software without restriction, includi [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string sampleSource {
            get {
                return ResourceManager.GetString("sampleSource", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #include &quot;speech.h&quot;
        ///
        ///#include &lt;map&gt;
        ///#include &lt;set&gt;
        ///#include &lt;string&gt;
        ///#include &lt;utility&gt;
        ///#include &lt;vector&gt;
        ///
        ///#include &quot;json.h&quot;
        ///#include &quot;rng.h&quot;
        ///
        ///static std::map&lt;std::string, std::vector&lt;SpeechBubble&gt;&gt; speech;
        ///
        ///static SpeechBubble nullSpeech = { no_translation( &quot;INVALID SPEECH&quot; ), 0 };
        ///
        ///void load_speech( const JsonObject &amp;jo )
        ///{
        ///    translation sound;
        ///    jo.read( &quot;sound&quot;, sound );
        ///    const int volume = jo.get_int( &quot;volume&quot; );
        ///    for( const std::string &amp;label : jo.get_tags( &quot;speaker&quot; ) )  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string sampleSource2 {
            get {
                return ResourceManager.GetString("sampleSource2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to lmao.
        /// </summary>
        internal static string test {
            get {
                return ResourceManager.GetString("test", resourceCulture);
            }
        }
    }
}