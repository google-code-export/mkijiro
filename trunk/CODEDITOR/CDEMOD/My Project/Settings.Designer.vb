﻿'------------------------------------------------------------------------------
' <auto-generated>
'     このコードはツールによって生成されました。
'     ランタイム バージョン:4.0.30319.239
'
'     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
'     コードが再生成されるときに損失したりします。
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Namespace My
    
    <Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0"),  _
     Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
    Partial Friend NotInheritable Class MySettings
        Inherits Global.System.Configuration.ApplicationSettingsBase
        
        Private Shared defaultInstance As MySettings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New MySettings()),MySettings)
        
#Region "My.Settings 自動保存機能"
#If _MyType = "WindowsForms" Then
    Private Shared addedHandler As Boolean

    Private Shared addedHandlerLockObject As New Object

    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Private Shared Sub AutoSaveSettings(ByVal sender As Global.System.Object, ByVal e As Global.System.EventArgs)
        If My.Application.SaveMySettingsOnExit Then
            My.Settings.Save()
        End If
    End Sub
#End If
#End Region
        
        Public Shared ReadOnly Property [Default]() As MySettings
            Get
                
#If _MyType = "WindowsForms" Then
               If Not addedHandler Then
                    SyncLock addedHandlerLockObject
                        If Not addedHandler Then
                            AddHandler My.Application.Shutdown, AddressOf AutoSaveSettings
                            addedHandler = True
                        End If
                    End SyncLock
                End If
#End If
                Return defaultInstance
            End Get
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("932")>  _
        Public Property MSCODEPAGE() As Integer
            Get
                Return CType(Me("MSCODEPAGE"),Integer)
            End Get
            Set
                Me("MSCODEPAGE") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("ＭＳ ゴシック, 9pt")>  _
        Public Property codetree() As Global.System.Drawing.Font
            Get
                Return CType(Me("codetree"),Global.System.Drawing.Font)
            End Get
            Set
                Me("codetree") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("ＭＳ ゴシック, 9pt")>  _
        Public Property CT_tb() As Global.System.Drawing.Font
            Get
                Return CType(Me("CT_tb"),Global.System.Drawing.Font)
            End Get
            Set
                Me("CT_tb") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("ＭＳ ゴシック, 9pt")>  _
        Public Property GID_tb() As Global.System.Drawing.Font
            Get
                Return CType(Me("GID_tb"),Global.System.Drawing.Font)
            End Get
            Set
                Me("GID_tb") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("ＭＳ ゴシック, 9pt")>  _
        Public Property GT_tb() As Global.System.Drawing.Font
            Get
                Return CType(Me("GT_tb"),Global.System.Drawing.Font)
            End Get
            Set
                Me("GT_tb") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("ＭＳ ゴシック, 9pt")>  _
        Public Property cmt_tb() As Global.System.Drawing.Font
            Get
                Return CType(Me("cmt_tb"),Global.System.Drawing.Font)
            End Get
            Set
                Me("cmt_tb") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("ＭＳ ゴシック, 9pt")>  _
        Public Property cl_tb() As Global.System.Drawing.Font
            Get
                Return CType(Me("cl_tb"),Global.System.Drawing.Font)
            End Get
            Set
                Me("cl_tb") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property TOP() As Boolean
            Get
                Return CType(Me("TOP"),Boolean)
            End Get
            Set
                Me("TOP") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property ERR() As Boolean
            Get
                Return CType(Me("ERR"),Boolean)
            End Get
            Set
                Me("ERR") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("IExplore.exe")>  _
        Public Property browser() As String
            Get
                Return CType(Me("browser"),String)
            End Get
            Set
                Me("browser") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property gridview() As Boolean
            Get
                Return CType(Me("gridview"),Boolean)
            End Get
            Set
                Me("gridview") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property lastcodepath() As String
            Get
                Return CType(Me("lastcodepath"),String)
            End Get
            Set
                Me("lastcodepath") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property gridvalueedit() As Boolean
            Get
                Return CType(Me("gridvalueedit"),Boolean)
            End Get
            Set
                Me("gridvalueedit") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("http://unzu127xp.pa.land.to/mogura/writelog.php?dl=http://unzu127xp.pa.land.to/da"& _ 
            "ta/IJIRO/CDEMOD/bin/Release/CDE_CP932_FM4.exe")>  _
        Public Property download() As String
            Get
                Return CType(Me("download"),String)
            End Get
            Set
                Me("download") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("C:\Program Files\Jane Style\Jane2ch.exe")>  _
        Public Property nichbrowser() As String
            Get
                Return CType(Me("nichbrowser"),String)
            End Get
            Set
                Me("nichbrowser") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property app8() As String
            Get
                Return CType(Me("app8"),String)
            End Get
            Set
                Me("app8") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property app9() As String
            Get
                Return CType(Me("app9"),String)
            End Get
            Set
                Me("app9") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property app10() As String
            Get
                Return CType(Me("app10"),String)
            End Get
            Set
                Me("app10") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property url8() As String
            Get
                Return CType(Me("url8"),String)
            End Get
            Set
                Me("url8") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property url9() As String
            Get
                Return CType(Me("url9"),String)
            End Get
            Set
                Me("url9") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property url10() As String
            Get
                Return CType(Me("url10"),String)
            End Get
            Set
                Me("url10") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property fixedform() As Boolean
            Get
                Return CType(Me("fixedform"),Boolean)
            End Get
            Set
                Me("fixedform") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("655")>  _
        Public Property mainyoko() As Integer
            Get
                Return CType(Me("mainyoko"),Integer)
            End Get
            Set
                Me("mainyoko") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("596")>  _
        Public Property maintate() As Integer
            Get
                Return CType(Me("maintate"),Integer)
            End Get
            Set
                Me("maintate") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property codepathwhensave() As Boolean
            Get
                Return CType(Me("codepathwhensave"),Boolean)
            End Get
            Set
                Me("codepathwhensave") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property updater() As Boolean
            Get
                Return CType(Me("updater"),Boolean)
            End Get
            Set
                Me("updater") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("192.168.1.10")>  _
        Public Property dhcpstart() As String
            Get
                Return CType(Me("dhcpstart"),String)
            End Get
            Set
                Me("dhcpstart") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("192.168.1.100")>  _
        Public Property dhcpend() As String
            Get
                Return CType(Me("dhcpend"),String)
            End Get
            Set
                Me("dhcpend") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property ftpdaemon() As Boolean
            Get
                Return CType(Me("ftpdaemon"),Boolean)
            End Get
            Set
                Me("ftpdaemon") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("/PICTURE/CWC")>  _
        Public Property ftpdir() As String
            Get
                Return CType(Me("ftpdir"),String)
            End Get
            Set
                Me("ftpdir") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("192.168.1.18")>  _
        Public Property staticip() As String
            Get
                Return CType(Me("staticip"),String)
            End Get
            Set
                Me("staticip") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property customwait() As Boolean
            Get
                Return CType(Me("customwait"),Boolean)
            End Get
            Set
                Me("customwait") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("100")>  _
        Public Property customsecond() As Integer
            Get
                Return CType(Me("customsecond"),Integer)
            End Get
            Set
                Me("customsecond") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property hbhash() As Boolean
            Get
                Return CType(Me("hbhash"),Boolean)
            End Get
            Set
                Me("hbhash") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property updatemode() As Boolean
            Get
                Return CType(Me("updatemode"),Boolean)
            End Get
            Set
                Me("updatemode") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property updatecomp() As Boolean
            Get
                Return CType(Me("updatecomp"),Boolean)
            End Get
            Set
                Me("updatecomp") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property updatesever() As Boolean
            Get
                Return CType(Me("updatesever"),Boolean)
            End Get
            Set
                Me("updatesever") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("100, 100")>  _
        Public Property mylocation() As Global.System.Drawing.Point
            Get
                Return CType(Me("mylocation"),Global.System.Drawing.Point)
            End Get
            Set
                Me("mylocation") = value
            End Set
        End Property
    End Class
End Namespace

Namespace My
    
    <Global.Microsoft.VisualBasic.HideModuleNameAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Module MySettingsProperty
        
        <Global.System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")>  _
        Friend ReadOnly Property Settings() As Global.CWcheat_Database_Editor.My.MySettings
            Get
                Return Global.CWcheat_Database_Editor.My.MySettings.Default
            End Get
        End Property
    End Module
End Namespace
