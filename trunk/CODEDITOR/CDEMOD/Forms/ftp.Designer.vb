<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ftp
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
        Me.FTPdir = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.IPbox = New System.Windows.Forms.TextBox()
        Me.daemonfinder = New System.Windows.Forms.CheckBox()
        Me.IP1 = New System.Windows.Forms.TextBox()
        Me.IP2 = New System.Windows.Forms.TextBox()
        Me.IP3 = New System.Windows.Forms.TextBox()
        Me.IP4 = New System.Windows.Forms.TextBox()
        Me.IP5 = New System.Windows.Forms.TextBox()
        Me.IP6 = New System.Windows.Forms.TextBox()
        Me.IP7 = New System.Windows.Forms.TextBox()
        Me.IP8 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.SECOND = New System.Windows.Forms.TextBox()
        Me.WAIT = New System.Windows.Forms.CheckBox()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'FTPdir
        '
        Me.FTPdir.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.FTPdir.Location = New System.Drawing.Point(29, 28)
        Me.FTPdir.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.FTPdir.MaxLength = 64
        Me.FTPdir.Name = "FTPdir"
        Me.FTPdir.Size = New System.Drawing.Size(224, 22)
        Me.FTPdir.TabIndex = 1
        Me.FTPdir.Text = "/PICTURE/CWC"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(81, 266)
        Me.Button1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(100, 29)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "OK"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'IPbox
        '
        Me.IPbox.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.IPbox.Location = New System.Drawing.Point(29, 80)
        Me.IPbox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.IPbox.MaxLength = 15
        Me.IPbox.Name = "IPbox"
        Me.IPbox.Size = New System.Drawing.Size(132, 22)
        Me.IPbox.TabIndex = 3
        Me.IPbox.Text = "192.168.1.18"
        '
        'daemonfinder
        '
        Me.daemonfinder.AutoSize = True
        Me.daemonfinder.Location = New System.Drawing.Point(15, 25)
        Me.daemonfinder.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.daemonfinder.Name = "daemonfinder"
        Me.daemonfinder.Size = New System.Drawing.Size(219, 24)
        Me.daemonfinder.TabIndex = 4
        Me.daemonfinder.Text = "DHCP範囲で検索する"
        Me.daemonfinder.UseVisualStyleBackColor = True
        '
        'IP1
        '
        Me.IP1.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.IP1.Location = New System.Drawing.Point(67, 52)
        Me.IP1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.IP1.MaxLength = 3
        Me.IP1.Name = "IP1"
        Me.IP1.Size = New System.Drawing.Size(36, 22)
        Me.IP1.TabIndex = 5
        Me.IP1.Text = "192"
        '
        'IP2
        '
        Me.IP2.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.IP2.Location = New System.Drawing.Point(112, 52)
        Me.IP2.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.IP2.MaxLength = 3
        Me.IP2.Name = "IP2"
        Me.IP2.Size = New System.Drawing.Size(35, 22)
        Me.IP2.TabIndex = 6
        Me.IP2.Text = "168"
        '
        'IP3
        '
        Me.IP3.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.IP3.Location = New System.Drawing.Point(156, 52)
        Me.IP3.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.IP3.MaxLength = 3
        Me.IP3.Name = "IP3"
        Me.IP3.Size = New System.Drawing.Size(29, 22)
        Me.IP3.TabIndex = 7
        Me.IP3.Text = "1"
        '
        'IP4
        '
        Me.IP4.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.IP4.Location = New System.Drawing.Point(195, 52)
        Me.IP4.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.IP4.MaxLength = 3
        Me.IP4.Name = "IP4"
        Me.IP4.Size = New System.Drawing.Size(36, 22)
        Me.IP4.TabIndex = 8
        Me.IP4.Text = "10"
        '
        'IP5
        '
        Me.IP5.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.IP5.Location = New System.Drawing.Point(67, 86)
        Me.IP5.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.IP5.MaxLength = 3
        Me.IP5.Name = "IP5"
        Me.IP5.Size = New System.Drawing.Size(36, 22)
        Me.IP5.TabIndex = 9
        Me.IP5.Text = "192"
        '
        'IP6
        '
        Me.IP6.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.IP6.Location = New System.Drawing.Point(112, 86)
        Me.IP6.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.IP6.MaxLength = 3
        Me.IP6.Name = "IP6"
        Me.IP6.Size = New System.Drawing.Size(35, 22)
        Me.IP6.TabIndex = 10
        Me.IP6.Text = "168"
        '
        'IP7
        '
        Me.IP7.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.IP7.Location = New System.Drawing.Point(156, 86)
        Me.IP7.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.IP7.MaxLength = 3
        Me.IP7.Name = "IP7"
        Me.IP7.Size = New System.Drawing.Size(29, 22)
        Me.IP7.TabIndex = 11
        Me.IP7.Text = "1"
        '
        'IP8
        '
        Me.IP8.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.IP8.Location = New System.Drawing.Point(195, 86)
        Me.IP8.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.IP8.MaxLength = 3
        Me.IP8.Name = "IP8"
        Me.IP8.Size = New System.Drawing.Size(36, 22)
        Me.IP8.TabIndex = 12
        Me.IP8.Text = "100"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 56)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(37, 15)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "開始"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 90)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(37, 15)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "終了"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(27, 61)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(172, 15)
        Me.Label3.TabIndex = 15
        Me.Label3.Text = "PSPFTPD IPアドレス(静的)"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(27, 9)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 15)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "CWDこまんど"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.IP8)
        Me.GroupBox1.Controls.Add(Me.IP7)
        Me.GroupBox1.Controls.Add(Me.IP6)
        Me.GroupBox1.Controls.Add(Me.IP5)
        Me.GroupBox1.Controls.Add(Me.IP4)
        Me.GroupBox1.Controls.Add(Me.IP3)
        Me.GroupBox1.Controls.Add(Me.IP2)
        Me.GroupBox1.Controls.Add(Me.IP1)
        Me.GroupBox1.Controls.Add(Me.daemonfinder)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 148)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox1.Size = New System.Drawing.Size(239, 118)
        Me.GroupBox1.TabIndex = 17
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "PSPFTPD検索"
        '
        'SECOND
        '
        Me.SECOND.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.SECOND.Location = New System.Drawing.Point(217, 112)
        Me.SECOND.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.SECOND.MaxLength = 4
        Me.SECOND.Name = "SECOND"
        Me.SECOND.Size = New System.Drawing.Size(45, 22)
        Me.SECOND.TabIndex = 18
        Me.SECOND.Text = "300"
        '
        'WAIT
        '
        Me.WAIT.AutoSize = True
        Me.WAIT.Location = New System.Drawing.Point(29, 115)
        Me.WAIT.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.WAIT.Name = "WAIT"
        Me.WAIT.Size = New System.Drawing.Size(178, 19)
        Me.WAIT.TabIndex = 19
        Me.WAIT.Text = "応答待ち時間（0.001秒)"
        Me.WAIT.UseVisualStyleBackColor = True
        '
        'ftp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(280, 310)
        Me.Controls.Add(Me.WAIT)
        Me.Controls.Add(Me.SECOND)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.IPbox)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.FTPdir)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "ftp"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "FTP設定"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents FTPdir As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents IPbox As System.Windows.Forms.TextBox
    Friend WithEvents daemonfinder As System.Windows.Forms.CheckBox
    Friend WithEvents IP1 As System.Windows.Forms.TextBox
    Friend WithEvents IP2 As System.Windows.Forms.TextBox
    Friend WithEvents IP3 As System.Windows.Forms.TextBox
    Friend WithEvents IP4 As System.Windows.Forms.TextBox
    Friend WithEvents IP5 As System.Windows.Forms.TextBox
    Friend WithEvents IP6 As System.Windows.Forms.TextBox
    Friend WithEvents IP7 As System.Windows.Forms.TextBox
    Friend WithEvents IP8 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents SECOND As System.Windows.Forms.TextBox
    Friend WithEvents WAIT As System.Windows.Forms.CheckBox
End Class
