<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form4
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form4))
        Me.IP2 = New System.Windows.Forms.TextBox()
        Me.IP1 = New System.Windows.Forms.TextBox()
        Me.IP3 = New System.Windows.Forms.TextBox()
        Me.IP4 = New System.Windows.Forms.TextBox()
        Me.IP5 = New System.Windows.Forms.TextBox()
        Me.IP6 = New System.Windows.Forms.TextBox()
        Me.IP7 = New System.Windows.Forms.TextBox()
        Me.IP8 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.daemonfinder = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.customwait = New System.Windows.Forms.CheckBox()
        Me.wait = New System.Windows.Forms.TextBox()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'IP2
        '
        resources.ApplyResources(Me.IP2, "IP2")
        Me.IP2.Name = "IP2"
        Me.IP2.ShortcutsEnabled = False
        '
        'IP1
        '
        resources.ApplyResources(Me.IP1, "IP1")
        Me.IP1.Name = "IP1"
        Me.IP1.ShortcutsEnabled = False
        '
        'IP3
        '
        resources.ApplyResources(Me.IP3, "IP3")
        Me.IP3.Name = "IP3"
        Me.IP3.ShortcutsEnabled = False
        '
        'IP4
        '
        resources.ApplyResources(Me.IP4, "IP4")
        Me.IP4.Name = "IP4"
        Me.IP4.ShortcutsEnabled = False
        '
        'IP5
        '
        resources.ApplyResources(Me.IP5, "IP5")
        Me.IP5.Name = "IP5"
        Me.IP5.ShortcutsEnabled = False
        '
        'IP6
        '
        resources.ApplyResources(Me.IP6, "IP6")
        Me.IP6.Name = "IP6"
        Me.IP6.ShortcutsEnabled = False
        '
        'IP7
        '
        resources.ApplyResources(Me.IP7, "IP7")
        Me.IP7.Name = "IP7"
        Me.IP7.ShortcutsEnabled = False
        '
        'IP8
        '
        resources.ApplyResources(Me.IP8, "IP8")
        Me.IP8.Name = "IP8"
        Me.IP8.ShortcutsEnabled = False
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'daemonfinder
        '
        resources.ApplyResources(Me.daemonfinder, "daemonfinder")
        Me.daemonfinder.Name = "daemonfinder"
        Me.daemonfinder.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Controls.Add(Me.IP8)
        Me.GroupBox1.Controls.Add(Me.IP4)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.IP3)
        Me.GroupBox1.Controls.Add(Me.IP5)
        Me.GroupBox1.Controls.Add(Me.IP6)
        Me.GroupBox1.Controls.Add(Me.IP7)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.IP2)
        Me.GroupBox1.Controls.Add(Me.IP1)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'Button1
        '
        resources.ApplyResources(Me.Button1, "Button1")
        Me.Button1.Name = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'customwait
        '
        resources.ApplyResources(Me.customwait, "customwait")
        Me.customwait.Name = "customwait"
        Me.customwait.UseVisualStyleBackColor = True
        '
        'wait
        '
        resources.ApplyResources(Me.wait, "wait")
        Me.wait.Name = "wait"
        Me.wait.ShortcutsEnabled = False
        '
        'Form4
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.wait)
        Me.Controls.Add(Me.customwait)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.daemonfinder)
        Me.Name = "Form4"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents IP2 As System.Windows.Forms.TextBox
    Friend WithEvents IP1 As System.Windows.Forms.TextBox
    Friend WithEvents IP3 As System.Windows.Forms.TextBox
    Friend WithEvents IP4 As System.Windows.Forms.TextBox
    Friend WithEvents IP5 As System.Windows.Forms.TextBox
    Friend WithEvents IP6 As System.Windows.Forms.TextBox
    Friend WithEvents IP7 As System.Windows.Forms.TextBox
    Friend WithEvents IP8 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents daemonfinder As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents customwait As System.Windows.Forms.CheckBox
    Friend WithEvents wait As System.Windows.Forms.TextBox
End Class
