<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class about
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    Friend WithEvents TableLayoutPanel As TableLayoutPanel
    Friend WithEvents LabelProductName As Label
    Friend WithEvents LabelVersion As Label
    Friend WithEvents LabelCompanyName As Label
    Friend WithEvents TextBoxDescription As TextBox
    Friend WithEvents OKButton As Button
    Friend WithEvents LabelCopyright As Label

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(about))
        TableLayoutPanel = New TableLayoutPanel()
        LogoPictureBox = New PictureBox()
        LabelProductName = New Label()
        LabelVersion = New Label()
        LabelCopyright = New Label()
        LabelCompanyName = New Label()
        TextBoxDescription = New TextBox()
        OKButton = New Button()
        Panel1 = New Panel()
        Panel2 = New Panel()
        TableLayoutPanel.SuspendLayout()
        CType(LogoPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' TableLayoutPanel
        ' 
        TableLayoutPanel.ColumnCount = 2
        TableLayoutPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33F))
        TableLayoutPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 67F))
        TableLayoutPanel.Controls.Add(LogoPictureBox, 0, 0)
        TableLayoutPanel.Controls.Add(LabelProductName, 1, 0)
        TableLayoutPanel.Controls.Add(LabelVersion, 1, 1)
        TableLayoutPanel.Controls.Add(LabelCopyright, 1, 2)
        TableLayoutPanel.Controls.Add(LabelCompanyName, 1, 3)
        TableLayoutPanel.Controls.Add(TextBoxDescription, 1, 4)
        TableLayoutPanel.Controls.Add(OKButton, 1, 5)
        TableLayoutPanel.Dock = DockStyle.Fill
        TableLayoutPanel.Location = New Point(10, 10)
        TableLayoutPanel.Margin = New Padding(4, 3, 4, 3)
        TableLayoutPanel.Name = "TableLayoutPanel"
        TableLayoutPanel.RowCount = 6
        TableLayoutPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 10F))
        TableLayoutPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 10F))
        TableLayoutPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 10F))
        TableLayoutPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 10F))
        TableLayoutPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        TableLayoutPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 10F))
        TableLayoutPanel.Size = New Size(463, 298)
        TableLayoutPanel.TabIndex = 0
        ' 
        ' LogoPictureBox
        ' 
        LogoPictureBox.Dock = DockStyle.Fill
        LogoPictureBox.ErrorImage = My.Resources.Resources.rlkktest_logo
        LogoPictureBox.Image = My.Resources.Resources.rlkktest_logo
        LogoPictureBox.Location = New Point(4, 3)
        LogoPictureBox.Margin = New Padding(4, 3, 4, 3)
        LogoPictureBox.Name = "LogoPictureBox"
        TableLayoutPanel.SetRowSpan(LogoPictureBox, 6)
        LogoPictureBox.Size = New Size(144, 292)
        LogoPictureBox.SizeMode = PictureBoxSizeMode.Zoom
        LogoPictureBox.TabIndex = 0
        LogoPictureBox.TabStop = False
        ' 
        ' LabelProductName
        ' 
        LabelProductName.Dock = DockStyle.Fill
        LabelProductName.Location = New Point(159, 0)
        LabelProductName.Margin = New Padding(7, 0, 4, 0)
        LabelProductName.MaximumSize = New Size(0, 20)
        LabelProductName.Name = "LabelProductName"
        LabelProductName.Size = New Size(300, 20)
        LabelProductName.TabIndex = 0
        LabelProductName.Text = "Produktname"
        LabelProductName.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' LabelVersion
        ' 
        LabelVersion.Dock = DockStyle.Fill
        LabelVersion.Location = New Point(159, 29)
        LabelVersion.Margin = New Padding(7, 0, 4, 0)
        LabelVersion.MaximumSize = New Size(0, 20)
        LabelVersion.Name = "LabelVersion"
        LabelVersion.Size = New Size(300, 20)
        LabelVersion.TabIndex = 0
        LabelVersion.Text = "Version"
        LabelVersion.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' LabelCopyright
        ' 
        LabelCopyright.Dock = DockStyle.Fill
        LabelCopyright.Location = New Point(159, 58)
        LabelCopyright.Margin = New Padding(7, 0, 4, 0)
        LabelCopyright.MaximumSize = New Size(0, 20)
        LabelCopyright.Name = "LabelCopyright"
        LabelCopyright.Size = New Size(300, 20)
        LabelCopyright.TabIndex = 0
        LabelCopyright.Text = "Copyright"
        LabelCopyright.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' LabelCompanyName
        ' 
        LabelCompanyName.Dock = DockStyle.Fill
        LabelCompanyName.Location = New Point(159, 87)
        LabelCompanyName.Margin = New Padding(7, 0, 4, 0)
        LabelCompanyName.MaximumSize = New Size(0, 20)
        LabelCompanyName.Name = "LabelCompanyName"
        LabelCompanyName.Size = New Size(300, 20)
        LabelCompanyName.TabIndex = 0
        LabelCompanyName.Text = "Firmenname"
        LabelCompanyName.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' TextBoxDescription
        ' 
        TextBoxDescription.Dock = DockStyle.Fill
        TextBoxDescription.Location = New Point(159, 119)
        TextBoxDescription.Margin = New Padding(7, 3, 4, 3)
        TextBoxDescription.Multiline = True
        TextBoxDescription.Name = "TextBoxDescription"
        TextBoxDescription.ReadOnly = True
        TextBoxDescription.ScrollBars = ScrollBars.Both
        TextBoxDescription.Size = New Size(300, 143)
        TextBoxDescription.TabIndex = 0
        TextBoxDescription.TabStop = False
        TextBoxDescription.Text = resources.GetString("TextBoxDescription.Text")
        ' 
        ' OKButton
        ' 
        OKButton.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        OKButton.DialogResult = DialogResult.Cancel
        OKButton.Location = New Point(371, 268)
        OKButton.Margin = New Padding(4, 3, 4, 3)
        OKButton.Name = "OKButton"
        OKButton.Size = New Size(88, 27)
        OKButton.TabIndex = 0
        OKButton.Text = "&OK"
        ' 
        ' Panel1
        ' 
        Panel1.BackgroundImage = My.Resources.Resources.logo_mst
        Panel1.BackgroundImageLayout = ImageLayout.Zoom
        Panel1.Location = New Point(12, 241)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(148, 72)
        Panel1.TabIndex = 1
        ' 
        ' Panel2
        ' 
        Panel2.BackgroundImage = My.Resources.Resources.kallelay
        Panel2.BackgroundImageLayout = ImageLayout.Zoom
        Panel2.Location = New Point(17, 17)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(148, 72)
        Panel2.TabIndex = 3
        ' 
        ' about
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.AliceBlue
        CancelButton = OKButton
        ClientSize = New Size(483, 318)
        Controls.Add(Panel2)
        Controls.Add(Panel1)
        Controls.Add(TableLayoutPanel)
        FormBorderStyle = FormBorderStyle.FixedDialog
        Margin = New Padding(4, 3, 4, 3)
        MaximizeBox = False
        MinimizeBox = False
        Name = "about"
        Padding = New Padding(10)
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "about"
        TableLayoutPanel.ResumeLayout(False)
        TableLayoutPanel.PerformLayout()
        CType(LogoPictureBox, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents LogoPictureBox As PictureBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
End Class
