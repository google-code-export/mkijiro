<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.NOINTRO_DIFF = New System.Windows.Forms.Button()
        Me.CVT_CLRMAEPRO = New System.Windows.Forms.Button()
        Me.ENJPN = New System.Windows.Forms.Button()
        Me.MERGE = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.CVT_TSV = New System.Windows.Forms.Button()
        Me.REDUMP_DIFF = New System.Windows.Forms.Button()
        Me.CVT_CSV = New System.Windows.Forms.Button()
        Me.GETHTML = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.CRCマスクToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CRCマスクToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.バージョンToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'NOINTRO_DIFF
        '
        Me.NOINTRO_DIFF.Font = New System.Drawing.Font("MS UI Gothic", 7.854546!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.NOINTRO_DIFF.Location = New System.Drawing.Point(11, 27)
        Me.NOINTRO_DIFF.Name = "NOINTRO_DIFF"
        Me.NOINTRO_DIFF.Size = New System.Drawing.Size(113, 26)
        Me.NOINTRO_DIFF.TabIndex = 0
        Me.NOINTRO_DIFF.Text = "NOINTRO_FIX"
        Me.NOINTRO_DIFF.UseVisualStyleBackColor = True
        '
        'CVT_CLRMAEPRO
        '
        Me.CVT_CLRMAEPRO.Font = New System.Drawing.Font("MS UI Gothic", 7.854546!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CVT_CLRMAEPRO.Location = New System.Drawing.Point(11, 90)
        Me.CVT_CLRMAEPRO.Name = "CVT_CLRMAEPRO"
        Me.CVT_CLRMAEPRO.Size = New System.Drawing.Size(113, 26)
        Me.CVT_CLRMAEPRO.TabIndex = 1
        Me.CVT_CLRMAEPRO.Text = "CMPROに変換"
        Me.CVT_CLRMAEPRO.UseVisualStyleBackColor = True
        '
        'ENJPN
        '
        Me.ENJPN.Font = New System.Drawing.Font("MS UI Gothic", 7.854546!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.ENJPN.Location = New System.Drawing.Point(11, 27)
        Me.ENJPN.Name = "ENJPN"
        Me.ENJPN.Size = New System.Drawing.Size(113, 26)
        Me.ENJPN.TabIndex = 2
        Me.ENJPN.Text = "英和名マージ"
        Me.ENJPN.UseVisualStyleBackColor = True
        '
        'MERGE
        '
        Me.MERGE.Font = New System.Drawing.Font("MS UI Gothic", 7.854546!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.MERGE.Location = New System.Drawing.Point(11, 60)
        Me.MERGE.Name = "MERGE"
        Me.MERGE.Size = New System.Drawing.Size(113, 24)
        Me.MERGE.TabIndex = 3
        Me.MERGE.Text = "重複チェック"
        Me.MERGE.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.CVT_TSV)
        Me.GroupBox1.Controls.Add(Me.REDUMP_DIFF)
        Me.GroupBox1.Controls.Add(Me.NOINTRO_DIFF)
        Me.GroupBox1.Controls.Add(Me.CVT_CSV)
        Me.GroupBox1.Controls.Add(Me.CVT_CLRMAEPRO)
        Me.GroupBox1.Location = New System.Drawing.Point(23, 32)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(143, 191)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "XML"
        '
        'CVT_TSV
        '
        Me.CVT_TSV.Location = New System.Drawing.Point(11, 151)
        Me.CVT_TSV.Name = "CVT_TSV"
        Me.CVT_TSV.Size = New System.Drawing.Size(113, 23)
        Me.CVT_TSV.TabIndex = 4
        Me.CVT_TSV.Text = "TSVに変換"
        Me.CVT_TSV.UseVisualStyleBackColor = True
        '
        'REDUMP_DIFF
        '
        Me.REDUMP_DIFF.Font = New System.Drawing.Font("MS UI Gothic", 7.854546!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.REDUMP_DIFF.Location = New System.Drawing.Point(11, 59)
        Me.REDUMP_DIFF.Name = "REDUMP_DIFF"
        Me.REDUMP_DIFF.Size = New System.Drawing.Size(113, 25)
        Me.REDUMP_DIFF.TabIndex = 2
        Me.REDUMP_DIFF.Text = "REDUMP_FIX"
        Me.REDUMP_DIFF.UseVisualStyleBackColor = True
        '
        'CVT_CSV
        '
        Me.CVT_CSV.Location = New System.Drawing.Point(11, 122)
        Me.CVT_CSV.Name = "CVT_CSV"
        Me.CVT_CSV.Size = New System.Drawing.Size(113, 23)
        Me.CVT_CSV.TabIndex = 3
        Me.CVT_CSV.Text = "CSVに変換"
        Me.CVT_CSV.UseVisualStyleBackColor = True
        '
        'GETHTML
        '
        Me.GETHTML.Font = New System.Drawing.Font("MS UI Gothic", 7.854546!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.GETHTML.Location = New System.Drawing.Point(11, 90)
        Me.GETHTML.Name = "GETHTML"
        Me.GETHTML.Size = New System.Drawing.Size(113, 26)
        Me.GETHTML.TabIndex = 3
        Me.GETHTML.Text = "REDUMP_WIKI"
        Me.GETHTML.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.GETHTML)
        Me.GroupBox2.Controls.Add(Me.ENJPN)
        Me.GroupBox2.Controls.Add(Me.MERGE)
        Me.GroupBox2.Location = New System.Drawing.Point(185, 32)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(143, 127)
        Me.GroupBox2.TabIndex = 5
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "CMPRO"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CRCマスクToolStripMenuItem, Me.バージョンToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(345, 29)
        Me.MenuStrip1.TabIndex = 6
        Me.MenuStrip1.Text = "CRCマスク設定"
        '
        'CRCマスクToolStripMenuItem
        '
        Me.CRCマスクToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CRCマスクToolStripMenuItem1})
        Me.CRCマスクToolStripMenuItem.Name = "CRCマスクToolStripMenuItem"
        Me.CRCマスクToolStripMenuItem.Size = New System.Drawing.Size(50, 25)
        Me.CRCマスクToolStripMenuItem.Text = "設定"
        '
        'CRCマスクToolStripMenuItem1
        '
        Me.CRCマスクToolStripMenuItem1.Name = "CRCマスクToolStripMenuItem1"
        Me.CRCマスクToolStripMenuItem1.Size = New System.Drawing.Size(164, 26)
        Me.CRCマスクToolStripMenuItem1.Text = "変換時の出力"
        '
        'バージョンToolStripMenuItem
        '
        Me.バージョンToolStripMenuItem.Name = "バージョンToolStripMenuItem"
        Me.バージョンToolStripMenuItem.Size = New System.Drawing.Size(92, 25)
        Me.バージョンToolStripMenuItem.Text = "バージョン"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(345, 240)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DB_FIXER"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents NOINTRO_DIFF As System.Windows.Forms.Button
    Friend WithEvents CVT_CLRMAEPRO As System.Windows.Forms.Button
    Friend WithEvents ENJPN As System.Windows.Forms.Button
    Friend WithEvents MERGE As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents REDUMP_DIFF As System.Windows.Forms.Button
    Friend WithEvents GETHTML As System.Windows.Forms.Button
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents CRCマスクToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents バージョンToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CVT_TSV As System.Windows.Forms.Button
    Friend WithEvents CVT_CSV As System.Windows.Forms.Button
    Friend WithEvents CRCマスクToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem

End Class
