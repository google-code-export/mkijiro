﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.239
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace bin2array.Properties {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("bin2array.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   厳密に型指定されたこのリソース クラスを使用して、すべての検索リソースに対し、
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
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
        ///   for (int i = 0; i &lt; bs.Length - kazu; i ++)
        ///                { に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string f1 {
            get {
                return ResourceManager.GetString("f1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   For i as integer = 0 To bs.Length - kazu
        /// に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string f2 {
            get {
                return ResourceManager.GetString("f2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Dim md5 As New System.Security.Cryptography.MD5CryptoServiceProvider()
        ///
        ///Dim bs As Byte() = md5.ComputeHash(data)
        ///
        ///Dim result As String = BitConverter.ToString(bs).ToUpper().Replace(&quot;-&quot;, &quot;&quot;) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string md {
            get {
                return ResourceManager.GetString("md", resourceCulture);
            }
        }
        
        /// <summary>
        ///   System.Security.Cryptography.MD5CryptoServiceProvider md5 =
        ///    new System.Security.Cryptography.MD5CryptoServiceProvider();
        ///byte[] bs = md5.ComputeHash(data);
        ///string result = BitConverter.ToString(bs).ToLower().Replace(&quot;-&quot;,&quot;&quot;); に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string md2 {
            get {
                return ResourceManager.GetString("md2", resourceCulture);
            }
        }
    }
}
