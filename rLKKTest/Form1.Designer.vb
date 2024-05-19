<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(Form1))
        PlotViewNyquist = New PlotView()
        MenuStrip1 = New MenuStrip()
        ToolStripMenuItem1 = New ToolStripMenuItem()
        DataToolStripMenuItem1 = New ToolStripMenuItem()
        RLKKParametersToolStripMenuItem = New ToolStripMenuItem()
        ExportToolStripMenuItem = New ToolStripMenuItem()
        DataToolStripMenuItem = New ToolStripMenuItem()
        RLKKParametersToolStripMenuItem1 = New ToolStripMenuItem()
        NyquistCurveToolStripMenuItem = New ToolStripMenuItem()
        DeviationsPlotToolStripMenuItem = New ToolStripMenuItem()
        RLKKPlotToolStripMenuItem = New ToolStripMenuItem()
        AboutToolStripMenuItem = New ToolStripMenuItem()
        PlotViewBodeMag = New PlotView()
        ButtonLoad = New Button()
        OpenFileDialog1 = New OpenFileDialog()
        Panel1 = New Panel()
        Button2 = New Button()
        DataGridViewImpedance = New DataGridView()
        CheckBoxLogScale = New CheckBox()
        CheckboxReverseNyq = New CheckBox()
        Filepathtext = New TextBox()
        CheckBoxPlotrLKK = New CheckBox()
        GroupBox1 = New GroupBox()
        Button4 = New Button()
        CheckBoxRinf = New CheckBox()
        CheckBoxR0 = New CheckBox()
        Label5 = New Label()
        ComboBoxWeight = New ComboBox()
        Label3 = New Label()
        rLKKparamsInfoLabel = New Label()
        Label4 = New Label()
        lambdaTextBox = New TextBox()
        Button3 = New Button()
        nDRTTextBox = New TextBox()
        Label2 = New Label()
        fmaxTextBox = New TextBox()
        Label1 = New Label()
        fminTextBox = New TextBox()
        Button1 = New Button()
        moreinfoLabel = New Label()
        fileinfLabel = New Label()
        SplitContainer1 = New SplitContainer()
        TabControl1 = New TabControl()
        TabPage1 = New TabPage()
        PlotViewDev = New PlotView()
        Panel2 = New Panel()
        ComboBoxRefDev = New ComboBox()
        Label7 = New Label()
        Label8 = New Label()
        Label6 = New Label()
        RadioButton1 = New RadioButton()
        RadioButtonPpm = New RadioButton()
        RadioButtonPercent = New RadioButton()
        CheckBox2perth = New CheckBox()
        CheckBox1perth = New CheckBox()
        CheckBoxDevMag = New CheckBox()
        CheckBoxDevImg = New CheckBox()
        CheckBoxDevReal = New CheckBox()
        TabPage2 = New TabPage()
        SplitContainer2 = New SplitContainer()
        PlotViewBodePhase = New PlotView()
        TabPage3 = New TabPage()
        PlotViewrLKK = New PlotView()
        TabPage4 = New TabPage()
        SaveFileDialog1 = New SaveFileDialog()
        OpenFileDialog2 = New OpenFileDialog()
        SaveFileDialog2 = New SaveFileDialog()
        MenuStrip1.SuspendLayout()
        Panel1.SuspendLayout()
        CType(DataGridViewImpedance, ComponentModel.ISupportInitialize).BeginInit()
        GroupBox1.SuspendLayout()
        CType(SplitContainer1, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer1.Panel1.SuspendLayout()
        SplitContainer1.Panel2.SuspendLayout()
        SplitContainer1.SuspendLayout()
        TabControl1.SuspendLayout()
        TabPage1.SuspendLayout()
        Panel2.SuspendLayout()
        TabPage2.SuspendLayout()
        CType(SplitContainer2, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer2.Panel1.SuspendLayout()
        SplitContainer2.Panel2.SuspendLayout()
        SplitContainer2.SuspendLayout()
        TabPage3.SuspendLayout()
        SuspendLayout()
        ' 
        ' PlotViewNyquist
        ' 
        PlotViewNyquist.BackColor = Color.White
        PlotViewNyquist.Dock = DockStyle.Fill
        PlotViewNyquist.Location = New Point(0, 0)
        PlotViewNyquist.Name = "PlotViewNyquist"
        PlotViewNyquist.PanCursor = Cursors.Hand
        PlotViewNyquist.Size = New Size(407, 442)
        PlotViewNyquist.TabIndex = 1
        PlotViewNyquist.ZoomHorizontalCursor = Cursors.SizeWE
        PlotViewNyquist.ZoomRectangleCursor = Cursors.SizeNWSE
        PlotViewNyquist.ZoomVerticalCursor = Cursors.SizeNS
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.Items.AddRange(New ToolStripItem() {ToolStripMenuItem1, ExportToolStripMenuItem, AboutToolStripMenuItem})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Size = New Size(1321, 24)
        MenuStrip1.TabIndex = 2
        MenuStrip1.Text = "MenuStrip1"
        ' 
        ' ToolStripMenuItem1
        ' 
        ToolStripMenuItem1.DropDownItems.AddRange(New ToolStripItem() {DataToolStripMenuItem1, RLKKParametersToolStripMenuItem})
        ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        ToolStripMenuItem1.Size = New Size(55, 20)
        ToolStripMenuItem1.Text = "&Import"
        ' 
        ' DataToolStripMenuItem1
        ' 
        DataToolStripMenuItem1.Name = "DataToolStripMenuItem1"
        DataToolStripMenuItem1.Size = New Size(160, 22)
        DataToolStripMenuItem1.Text = "Data"
        ' 
        ' RLKKParametersToolStripMenuItem
        ' 
        RLKKParametersToolStripMenuItem.Name = "RLKKParametersToolStripMenuItem"
        RLKKParametersToolStripMenuItem.Size = New Size(160, 22)
        RLKKParametersToolStripMenuItem.Text = "rLKK parameters"
        ' 
        ' ExportToolStripMenuItem
        ' 
        ExportToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {DataToolStripMenuItem, RLKKParametersToolStripMenuItem1, NyquistCurveToolStripMenuItem, DeviationsPlotToolStripMenuItem, RLKKPlotToolStripMenuItem})
        ExportToolStripMenuItem.Name = "ExportToolStripMenuItem"
        ExportToolStripMenuItem.Size = New Size(53, 20)
        ExportToolStripMenuItem.Text = "&Export"
        ' 
        ' DataToolStripMenuItem
        ' 
        DataToolStripMenuItem.Name = "DataToolStripMenuItem"
        DataToolStripMenuItem.Size = New Size(160, 22)
        DataToolStripMenuItem.Text = "&Data"
        ' 
        ' RLKKParametersToolStripMenuItem1
        ' 
        RLKKParametersToolStripMenuItem1.Name = "RLKKParametersToolStripMenuItem1"
        RLKKParametersToolStripMenuItem1.Size = New Size(160, 22)
        RLKKParametersToolStripMenuItem1.Text = "rLKK parameters"
        ' 
        ' NyquistCurveToolStripMenuItem
        ' 
        NyquistCurveToolStripMenuItem.Name = "NyquistCurveToolStripMenuItem"
        NyquistCurveToolStripMenuItem.Size = New Size(160, 22)
        NyquistCurveToolStripMenuItem.Text = "&Nyquist Curve"
        ' 
        ' DeviationsPlotToolStripMenuItem
        ' 
        DeviationsPlotToolStripMenuItem.Name = "DeviationsPlotToolStripMenuItem"
        DeviationsPlotToolStripMenuItem.Size = New Size(160, 22)
        DeviationsPlotToolStripMenuItem.Text = "Deviations plot"
        ' 
        ' RLKKPlotToolStripMenuItem
        ' 
        RLKKPlotToolStripMenuItem.Name = "RLKKPlotToolStripMenuItem"
        RLKKPlotToolStripMenuItem.Size = New Size(160, 22)
        RLKKPlotToolStripMenuItem.Text = "rLKK plot"
        ' 
        ' AboutToolStripMenuItem
        ' 
        AboutToolStripMenuItem.Image = My.Resources.Resources.kallelay
        AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        AboutToolStripMenuItem.Size = New Size(68, 20)
        AboutToolStripMenuItem.Text = "&About"
        ' 
        ' PlotViewBodeMag
        ' 
        PlotViewBodeMag.BackColor = Color.White
        PlotViewBodeMag.Dock = DockStyle.Fill
        PlotViewBodeMag.Location = New Point(0, 0)
        PlotViewBodeMag.Name = "PlotViewBodeMag"
        PlotViewBodeMag.PanCursor = Cursors.Hand
        PlotViewBodeMag.Size = New Size(518, 180)
        PlotViewBodeMag.TabIndex = 2
        PlotViewBodeMag.ZoomHorizontalCursor = Cursors.SizeWE
        PlotViewBodeMag.ZoomRectangleCursor = Cursors.SizeNWSE
        PlotViewBodeMag.ZoomVerticalCursor = Cursors.SizeNS
        ' 
        ' ButtonLoad
        ' 
        ButtonLoad.Location = New Point(7, 3)
        ButtonLoad.Name = "ButtonLoad"
        ButtonLoad.Size = New Size(51, 24)
        ButtonLoad.TabIndex = 3
        ButtonLoad.Text = "&load"
        ButtonLoad.UseVisualStyleBackColor = True
        ' 
        ' OpenFileDialog1
        ' 
        OpenFileDialog1.FileName = "OpenFileDialog1"
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(Button2)
        Panel1.Controls.Add(DataGridViewImpedance)
        Panel1.Controls.Add(CheckBoxLogScale)
        Panel1.Controls.Add(CheckboxReverseNyq)
        Panel1.Controls.Add(Filepathtext)
        Panel1.Controls.Add(CheckBoxPlotrLKK)
        Panel1.Controls.Add(ButtonLoad)
        Panel1.Controls.Add(GroupBox1)
        Panel1.Controls.Add(Button1)
        Panel1.Controls.Add(moreinfoLabel)
        Panel1.Controls.Add(fileinfLabel)
        Panel1.Dock = DockStyle.Right
        Panel1.Location = New Point(943, 24)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(378, 442)
        Panel1.TabIndex = 4
        ' 
        ' Button2
        ' 
        Button2.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Button2.Location = New Point(52, 195)
        Button2.Name = "Button2"
        Button2.Size = New Size(154, 28)
        Button2.TabIndex = 19
        Button2.Text = "Select > threshold"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' DataGridViewImpedance
        ' 
        DataGridViewImpedance.AllowUserToAddRows = False
        DataGridViewCellStyle1.BackColor = SystemColors.ButtonFace
        DataGridViewImpedance.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        DataGridViewImpedance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = Color.Honeydew
        DataGridViewCellStyle2.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular, GraphicsUnit.Point)
        DataGridViewCellStyle2.ForeColor = SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = DataGridViewTriState.True
        DataGridViewImpedance.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        DataGridViewImpedance.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewImpedance.Dock = DockStyle.Bottom
        DataGridViewImpedance.Location = New Point(0, 282)
        DataGridViewImpedance.Name = "DataGridViewImpedance"
        DataGridViewImpedance.RowTemplate.Height = 25
        DataGridViewImpedance.Size = New Size(378, 160)
        DataGridViewImpedance.TabIndex = 1
        ' 
        ' CheckBoxLogScale
        ' 
        CheckBoxLogScale.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        CheckBoxLogScale.AutoSize = True
        CheckBoxLogScale.Checked = True
        CheckBoxLogScale.CheckState = CheckState.Checked
        CheckBoxLogScale.Location = New Point(131, 33)
        CheckBoxLogScale.Name = "CheckBoxLogScale"
        CheckBoxLogScale.Size = New Size(75, 19)
        CheckBoxLogScale.TabIndex = 18
        CheckBoxLogScale.Text = "Log scale"
        CheckBoxLogScale.UseVisualStyleBackColor = True
        ' 
        ' CheckboxReverseNyq
        ' 
        CheckboxReverseNyq.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        CheckboxReverseNyq.AutoSize = True
        CheckboxReverseNyq.Checked = True
        CheckboxReverseNyq.CheckState = CheckState.Checked
        CheckboxReverseNyq.Location = New Point(238, 33)
        CheckboxReverseNyq.Name = "CheckboxReverseNyq"
        CheckboxReverseNyq.Size = New Size(137, 19)
        CheckboxReverseNyq.TabIndex = 5
        CheckboxReverseNyq.Text = "Flip Y-axis in Nyquist"
        CheckboxReverseNyq.UseVisualStyleBackColor = True
        ' 
        ' Filepathtext
        ' 
        Filepathtext.Location = New Point(69, 4)
        Filepathtext.Name = "Filepathtext"
        Filepathtext.Size = New Size(299, 23)
        Filepathtext.TabIndex = 4
        ' 
        ' CheckBoxPlotrLKK
        ' 
        CheckBoxPlotrLKK.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        CheckBoxPlotrLKK.AutoSize = True
        CheckBoxPlotrLKK.Checked = True
        CheckBoxPlotrLKK.CheckState = CheckState.Checked
        CheckBoxPlotrLKK.Location = New Point(13, 33)
        CheckBoxPlotrLKK.Name = "CheckBoxPlotrLKK"
        CheckBoxPlotrLKK.Size = New Size(74, 19)
        CheckBoxPlotrLKK.TabIndex = 5
        CheckBoxPlotrLKK.Text = "Plot rLKK"
        CheckBoxPlotrLKK.UseVisualStyleBackColor = True
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        GroupBox1.Controls.Add(Button4)
        GroupBox1.Controls.Add(CheckBoxRinf)
        GroupBox1.Controls.Add(CheckBoxR0)
        GroupBox1.Controls.Add(Label5)
        GroupBox1.Controls.Add(ComboBoxWeight)
        GroupBox1.Controls.Add(Label3)
        GroupBox1.Controls.Add(rLKKparamsInfoLabel)
        GroupBox1.Controls.Add(Label4)
        GroupBox1.Controls.Add(lambdaTextBox)
        GroupBox1.Controls.Add(Button3)
        GroupBox1.Controls.Add(nDRTTextBox)
        GroupBox1.Controls.Add(Label2)
        GroupBox1.Controls.Add(fmaxTextBox)
        GroupBox1.Controls.Add(Label1)
        GroupBox1.Controls.Add(fminTextBox)
        GroupBox1.Location = New Point(7, 61)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(364, 131)
        GroupBox1.TabIndex = 16
        GroupBox1.TabStop = False
        GroupBox1.Text = "rLKK parameters"
        ' 
        ' Button4
        ' 
        Button4.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Button4.AutoEllipsis = True
        Button4.BackColor = Color.SeaGreen
        Button4.FlatStyle = FlatStyle.Flat
        Button4.Font = New Font("Script MT Bold", 6.0F, FontStyle.Bold, GraphicsUnit.Point)
        Button4.Location = New Point(102, 0)
        Button4.Name = "Button4"
        Button4.Size = New Size(10, 10)
        Button4.TabIndex = 21
        Button4.Text = "--"
        Button4.UseVisualStyleBackColor = False
        ' 
        ' CheckBoxRinf
        ' 
        CheckBoxRinf.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        CheckBoxRinf.AutoSize = True
        CheckBoxRinf.Location = New Point(289, 78)
        CheckBoxRinf.Name = "CheckBoxRinf"
        CheckBoxRinf.Size = New Size(52, 19)
        CheckBoxRinf.TabIndex = 20
        CheckBoxRinf.Text = "R_inf"
        CheckBoxRinf.UseVisualStyleBackColor = True
        ' 
        ' CheckBoxR0
        ' 
        CheckBoxR0.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        CheckBoxR0.AutoSize = True
        CheckBoxR0.Location = New Point(234, 78)
        CheckBoxR0.Name = "CheckBoxR0"
        CheckBoxR0.Size = New Size(44, 19)
        CheckBoxR0.TabIndex = 19
        CheckBoxR0.Text = "R_0"
        CheckBoxR0.UseVisualStyleBackColor = True
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(4, 79)
        Label5.Name = "Label5"
        Label5.Size = New Size(53, 15)
        Label5.TabIndex = 17
        Label5.Text = "Weights:"
        ' 
        ' ComboBoxWeight
        ' 
        ComboBoxWeight.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBoxWeight.FormattingEnabled = True
        ComboBoxWeight.Items.AddRange(New Object() {"Lambda", "Lambda/|Z|"})
        ComboBoxWeight.Location = New Point(83, 74)
        ComboBoxWeight.Name = "ComboBoxWeight"
        ComboBoxWeight.Size = New Size(95, 23)
        ComboBoxWeight.TabIndex = 16
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(2, 48)
        Label3.Name = "Label3"
        Label3.Size = New Size(73, 15)
        Label3.TabIndex = 13
        Label3.Text = "DRT freq pts:"
        ' 
        ' rLKKparamsInfoLabel
        ' 
        rLKKparamsInfoLabel.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        rLKKparamsInfoLabel.AutoSize = True
        rLKKparamsInfoLabel.Location = New Point(6, 101)
        rLKKparamsInfoLabel.Name = "rLKKparamsInfoLabel"
        rLKKparamsInfoLabel.Size = New Size(28, 15)
        rLKKparamsInfoLabel.TabIndex = 6
        rLKKparamsInfoLabel.Text = "info"
        ' 
        ' Label4
        ' 
        Label4.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label4.AutoSize = True
        Label4.Location = New Point(200, 48)
        Label4.Name = "Label4"
        Label4.Size = New Size(50, 15)
        Label4.TabIndex = 15
        Label4.Text = "lambda:"
        ' 
        ' lambdaTextBox
        ' 
        lambdaTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lambdaTextBox.Location = New Point(259, 45)
        lambdaTextBox.Name = "lambdaTextBox"
        lambdaTextBox.Size = New Size(92, 23)
        lambdaTextBox.TabIndex = 14
        lambdaTextBox.Text = "1e-3"
        ' 
        ' Button3
        ' 
        Button3.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Button3.AutoEllipsis = True
        Button3.BackColor = Color.Tomato
        Button3.FlatStyle = FlatStyle.Flat
        Button3.Font = New Font("Script MT Bold", 6.0F, FontStyle.Bold, GraphicsUnit.Point)
        Button3.Location = New Point(118, 0)
        Button3.Name = "Button3"
        Button3.Size = New Size(10, 10)
        Button3.TabIndex = 17
        Button3.Text = "--"
        Button3.UseVisualStyleBackColor = False
        ' 
        ' nDRTTextBox
        ' 
        nDRTTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        nDRTTextBox.Location = New Point(81, 45)
        nDRTTextBox.Name = "nDRTTextBox"
        nDRTTextBox.Size = New Size(95, 23)
        nDRTTextBox.TabIndex = 12
        nDRTTextBox.Text = "100"
        ' 
        ' Label2
        ' 
        Label2.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label2.AutoSize = True
        Label2.Location = New Point(210, 16)
        Label2.Name = "Label2"
        Label2.Size = New Size(40, 15)
        Label2.TabIndex = 11
        Label2.Text = "f max:"
        ' 
        ' fmaxTextBox
        ' 
        fmaxTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        fmaxTextBox.Location = New Point(259, 13)
        fmaxTextBox.Name = "fmaxTextBox"
        fmaxTextBox.Size = New Size(95, 23)
        fmaxTextBox.TabIndex = 10
        fmaxTextBox.Text = "1e8"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(6, 19)
        Label1.Name = "Label1"
        Label1.Size = New Size(38, 15)
        Label1.TabIndex = 9
        Label1.Text = "f min:"
        ' 
        ' fminTextBox
        ' 
        fminTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        fminTextBox.Location = New Point(81, 16)
        fminTextBox.Name = "fminTextBox"
        fminTextBox.Size = New Size(92, 23)
        fminTextBox.TabIndex = 8
        fminTextBox.Text = "1e-8"
        ' 
        ' Button1
        ' 
        Button1.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Button1.Location = New Point(212, 195)
        Button1.Name = "Button1"
        Button1.Size = New Size(154, 28)
        Button1.TabIndex = 17
        Button1.Text = "invert imaginary values"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' moreinfoLabel
        ' 
        moreinfoLabel.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        moreinfoLabel.AutoSize = True
        moreinfoLabel.Location = New Point(13, 264)
        moreinfoLabel.Name = "moreinfoLabel"
        moreinfoLabel.Size = New Size(28, 15)
        moreinfoLabel.TabIndex = 7
        moreinfoLabel.Text = "info"
        ' 
        ' fileinfLabel
        ' 
        fileinfLabel.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        fileinfLabel.AutoSize = True
        fileinfLabel.Location = New Point(13, 237)
        fileinfLabel.Name = "fileinfLabel"
        fileinfLabel.Size = New Size(28, 15)
        fileinfLabel.TabIndex = 6
        fileinfLabel.Text = "info"
        ' 
        ' SplitContainer1
        ' 
        SplitContainer1.Dock = DockStyle.Fill
        SplitContainer1.Location = New Point(0, 24)
        SplitContainer1.Name = "SplitContainer1"
        ' 
        ' SplitContainer1.Panel1
        ' 
        SplitContainer1.Panel1.Controls.Add(PlotViewNyquist)
        ' 
        ' SplitContainer1.Panel2
        ' 
        SplitContainer1.Panel2.Controls.Add(TabControl1)
        SplitContainer1.Size = New Size(943, 442)
        SplitContainer1.SplitterDistance = 407
        SplitContainer1.TabIndex = 5
        ' 
        ' TabControl1
        ' 
        TabControl1.Controls.Add(TabPage1)
        TabControl1.Controls.Add(TabPage2)
        TabControl1.Controls.Add(TabPage3)
        TabControl1.Controls.Add(TabPage4)
        TabControl1.Dock = DockStyle.Fill
        TabControl1.Location = New Point(0, 0)
        TabControl1.Name = "TabControl1"
        TabControl1.SelectedIndex = 0
        TabControl1.Size = New Size(532, 442)
        TabControl1.TabIndex = 3
        ' 
        ' TabPage1
        ' 
        TabPage1.Controls.Add(PlotViewDev)
        TabPage1.Controls.Add(Panel2)
        TabPage1.Location = New Point(4, 24)
        TabPage1.Name = "TabPage1"
        TabPage1.Padding = New Padding(3)
        TabPage1.Size = New Size(524, 414)
        TabPage1.TabIndex = 0
        TabPage1.Text = "Deviations"
        TabPage1.UseVisualStyleBackColor = True
        ' 
        ' PlotViewDev
        ' 
        PlotViewDev.BackColor = Color.White
        PlotViewDev.Dock = DockStyle.Fill
        PlotViewDev.Location = New Point(3, 3)
        PlotViewDev.Name = "PlotViewDev"
        PlotViewDev.PanCursor = Cursors.Hand
        PlotViewDev.Size = New Size(518, 324)
        PlotViewDev.TabIndex = 3
        PlotViewDev.ZoomHorizontalCursor = Cursors.SizeWE
        PlotViewDev.ZoomRectangleCursor = Cursors.SizeNWSE
        PlotViewDev.ZoomVerticalCursor = Cursors.SizeNS
        ' 
        ' Panel2
        ' 
        Panel2.BackColor = Color.AliceBlue
        Panel2.Controls.Add(ComboBoxRefDev)
        Panel2.Controls.Add(Label7)
        Panel2.Controls.Add(Label8)
        Panel2.Controls.Add(Label6)
        Panel2.Controls.Add(RadioButton1)
        Panel2.Controls.Add(RadioButtonPpm)
        Panel2.Controls.Add(RadioButtonPercent)
        Panel2.Controls.Add(CheckBox2perth)
        Panel2.Controls.Add(CheckBox1perth)
        Panel2.Controls.Add(CheckBoxDevMag)
        Panel2.Controls.Add(CheckBoxDevImg)
        Panel2.Controls.Add(CheckBoxDevReal)
        Panel2.Dock = DockStyle.Bottom
        Panel2.Location = New Point(3, 327)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(518, 84)
        Panel2.TabIndex = 4
        ' 
        ' ComboBoxRefDev
        ' 
        ComboBoxRefDev.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBoxRefDev.FormattingEnabled = True
        ComboBoxRefDev.Items.AddRange(New Object() {"Real/Imaginary", "Magnitude"})
        ComboBoxRefDev.Location = New Point(305, 30)
        ComboBoxRefDev.Name = "ComboBoxRefDev"
        ComboBoxRefDev.Size = New Size(133, 23)
        ComboBoxRefDev.TabIndex = 17
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(14, 64)
        Label7.Name = "Label7"
        Label7.Size = New Size(32, 15)
        Label7.TabIndex = 7
        Label7.Text = "Unit:"
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(237, 34)
        Label8.Name = "Label8"
        Label8.Size = New Size(62, 15)
        Label8.TabIndex = 7
        Label8.Text = "Reference:"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(15, 6)
        Label6.Name = "Label6"
        Label6.Size = New Size(31, 15)
        Label6.TabIndex = 7
        Label6.Text = "Plot:"
        ' 
        ' RadioButton1
        ' 
        RadioButton1.AutoSize = True
        RadioButton1.Location = New Point(237, 62)
        RadioButton1.Name = "RadioButton1"
        RadioButton1.Size = New Size(52, 19)
        RadioButton1.TabIndex = 6
        RadioButton1.Text = "Ohm"
        RadioButton1.UseVisualStyleBackColor = True
        ' 
        ' RadioButtonPpm
        ' 
        RadioButtonPpm.AutoSize = True
        RadioButtonPpm.Location = New Point(171, 62)
        RadioButtonPpm.Name = "RadioButtonPpm"
        RadioButtonPpm.Size = New Size(50, 19)
        RadioButtonPpm.TabIndex = 5
        RadioButtonPpm.Text = "ppm"
        RadioButtonPpm.UseVisualStyleBackColor = True
        ' 
        ' RadioButtonPercent
        ' 
        RadioButtonPercent.AutoSize = True
        RadioButtonPercent.Checked = True
        RadioButtonPercent.Location = New Point(94, 62)
        RadioButtonPercent.Name = "RadioButtonPercent"
        RadioButtonPercent.Size = New Size(65, 19)
        RadioButtonPercent.TabIndex = 4
        RadioButtonPercent.TabStop = True
        RadioButtonPercent.Text = "Percent"
        RadioButtonPercent.UseVisualStyleBackColor = True
        ' 
        ' CheckBox2perth
        ' 
        CheckBox2perth.AutoSize = True
        CheckBox2perth.Location = New Point(116, 35)
        CheckBox2perth.Name = "CheckBox2perth"
        CheckBox2perth.Size = New Size(95, 19)
        CheckBox2perth.TabIndex = 3
        CheckBox2perth.Text = "2% threshold"
        CheckBox2perth.UseVisualStyleBackColor = True
        ' 
        ' CheckBox1perth
        ' 
        CheckBox1perth.AutoSize = True
        CheckBox1perth.Location = New Point(15, 34)
        CheckBox1perth.Name = "CheckBox1perth"
        CheckBox1perth.Size = New Size(95, 19)
        CheckBox1perth.TabIndex = 3
        CheckBox1perth.Text = "1% threshold"
        CheckBox1perth.UseVisualStyleBackColor = True
        ' 
        ' CheckBoxDevMag
        ' 
        CheckBoxDevMag.AutoSize = True
        CheckBoxDevMag.Checked = True
        CheckBoxDevMag.CheckState = CheckState.Checked
        CheckBoxDevMag.Location = New Point(277, 6)
        CheckBoxDevMag.Name = "CheckBoxDevMag"
        CheckBoxDevMag.Size = New Size(84, 19)
        CheckBoxDevMag.TabIndex = 2
        CheckBoxDevMag.Text = "Magnitude"
        CheckBoxDevMag.UseVisualStyleBackColor = True
        ' 
        ' CheckBoxDevImg
        ' 
        CheckBoxDevImg.AutoSize = True
        CheckBoxDevImg.Location = New Point(171, 6)
        CheckBoxDevImg.Name = "CheckBoxDevImg"
        CheckBoxDevImg.Size = New Size(79, 19)
        CheckBoxDevImg.TabIndex = 1
        CheckBoxDevImg.Text = "Imaginary"
        CheckBoxDevImg.UseVisualStyleBackColor = True
        ' 
        ' CheckBoxDevReal
        ' 
        CheckBoxDevReal.AutoSize = True
        CheckBoxDevReal.Location = New Point(94, 6)
        CheckBoxDevReal.Name = "CheckBoxDevReal"
        CheckBoxDevReal.Size = New Size(48, 19)
        CheckBoxDevReal.TabIndex = 0
        CheckBoxDevReal.Text = "Real"
        CheckBoxDevReal.UseVisualStyleBackColor = True
        ' 
        ' TabPage2
        ' 
        TabPage2.Controls.Add(SplitContainer2)
        TabPage2.Location = New Point(4, 24)
        TabPage2.Name = "TabPage2"
        TabPage2.Padding = New Padding(3)
        TabPage2.Size = New Size(524, 414)
        TabPage2.TabIndex = 1
        TabPage2.Text = "Bode"
        TabPage2.UseVisualStyleBackColor = True
        ' 
        ' SplitContainer2
        ' 
        SplitContainer2.Dock = DockStyle.Fill
        SplitContainer2.Location = New Point(3, 3)
        SplitContainer2.Name = "SplitContainer2"
        SplitContainer2.Orientation = Orientation.Horizontal
        ' 
        ' SplitContainer2.Panel1
        ' 
        SplitContainer2.Panel1.Controls.Add(PlotViewBodeMag)
        ' 
        ' SplitContainer2.Panel2
        ' 
        SplitContainer2.Panel2.Controls.Add(PlotViewBodePhase)
        SplitContainer2.Size = New Size(518, 408)
        SplitContainer2.SplitterDistance = 180
        SplitContainer2.TabIndex = 3
        ' 
        ' PlotViewBodePhase
        ' 
        PlotViewBodePhase.BackColor = Color.White
        PlotViewBodePhase.Dock = DockStyle.Fill
        PlotViewBodePhase.Location = New Point(0, 0)
        PlotViewBodePhase.Name = "PlotViewBodePhase"
        PlotViewBodePhase.PanCursor = Cursors.Hand
        PlotViewBodePhase.Size = New Size(518, 224)
        PlotViewBodePhase.TabIndex = 3
        PlotViewBodePhase.ZoomHorizontalCursor = Cursors.SizeWE
        PlotViewBodePhase.ZoomRectangleCursor = Cursors.SizeNWSE
        PlotViewBodePhase.ZoomVerticalCursor = Cursors.SizeNS
        ' 
        ' TabPage3
        ' 
        TabPage3.Controls.Add(PlotViewrLKK)
        TabPage3.Location = New Point(4, 24)
        TabPage3.Name = "TabPage3"
        TabPage3.Size = New Size(524, 414)
        TabPage3.TabIndex = 2
        TabPage3.Text = "rLKK results"
        TabPage3.UseVisualStyleBackColor = True
        ' 
        ' PlotViewrLKK
        ' 
        PlotViewrLKK.BackColor = Color.White
        PlotViewrLKK.Dock = DockStyle.Fill
        PlotViewrLKK.Location = New Point(0, 0)
        PlotViewrLKK.Name = "PlotViewrLKK"
        PlotViewrLKK.PanCursor = Cursors.Hand
        PlotViewrLKK.Size = New Size(524, 414)
        PlotViewrLKK.TabIndex = 3
        PlotViewrLKK.ZoomHorizontalCursor = Cursors.SizeWE
        PlotViewrLKK.ZoomRectangleCursor = Cursors.SizeNWSE
        PlotViewrLKK.ZoomVerticalCursor = Cursors.SizeNS
        ' 
        ' TabPage4
        ' 
        TabPage4.Location = New Point(4, 24)
        TabPage4.Name = "TabPage4"
        TabPage4.Size = New Size(524, 414)
        TabPage4.TabIndex = 3
        TabPage4.Text = "Data"
        TabPage4.UseVisualStyleBackColor = True
        ' 
        ' OpenFileDialog2
        ' 
        OpenFileDialog2.FileName = "OpenFileDialog2"
        ' 
        ' Form1
        ' 
        AllowDrop = True
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.AliceBlue
        ClientSize = New Size(1321, 466)
        Controls.Add(SplitContainer1)
        Controls.Add(Panel1)
        Controls.Add(MenuStrip1)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MainMenuStrip = MenuStrip1
        Name = "Form1"
        Text = "regularized Linear Kramers-Kronig Test 1.0"
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        CType(DataGridViewImpedance, ComponentModel.ISupportInitialize).EndInit()
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        SplitContainer1.Panel1.ResumeLayout(False)
        SplitContainer1.Panel2.ResumeLayout(False)
        CType(SplitContainer1, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer1.ResumeLayout(False)
        TabControl1.ResumeLayout(False)
        TabPage1.ResumeLayout(False)
        Panel2.ResumeLayout(False)
        Panel2.PerformLayout()
        TabPage2.ResumeLayout(False)
        SplitContainer2.Panel1.ResumeLayout(False)
        SplitContainer2.Panel2.ResumeLayout(False)
        CType(SplitContainer2, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer2.ResumeLayout(False)
        TabPage3.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents PlotViewNyquist As PlotView
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents PlotViewBodeMag As PlotView
    Friend WithEvents ButtonLoad As Button
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents Panel1 As Panel
    Friend WithEvents DataGridViewImpedance As DataGridView
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents PlotViewDev As PlotView
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents Filepathtext As TextBox
    Friend WithEvents CheckboxReverseNyq As CheckBox
    Friend WithEvents fileinfLabel As Label
    Friend WithEvents moreinfoLabel As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents lambdaTextBox As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents nDRTTextBox As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents fmaxTextBox As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents fminTextBox As TextBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents rLKKparamsInfoLabel As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents PlotViewrLKK As PlotView
    Friend WithEvents CheckBoxPlotrLKK As CheckBox
    Friend WithEvents Label5 As Label
    Friend WithEvents ComboBoxWeight As ComboBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents CheckBox2perth As CheckBox
    Friend WithEvents CheckBox1perth As CheckBox
    Friend WithEvents CheckBoxDevMag As CheckBox
    Friend WithEvents CheckBoxDevImg As CheckBox
    Friend WithEvents CheckBoxDevReal As CheckBox
    Friend WithEvents CheckBoxLogScale As CheckBox
    Friend WithEvents SplitContainer2 As SplitContainer
    Friend WithEvents PlotViewBodePhase As PlotView
    Friend WithEvents RadioButtonPpm As RadioButton
    Friend WithEvents RadioButtonPercent As RadioButton
    Friend WithEvents RadioButton1 As RadioButton
    Friend WithEvents ComboBoxRefDev As ComboBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents CheckBoxR0 As CheckBox
    Friend WithEvents CheckBoxRinf As CheckBox
    Friend WithEvents ExportToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TabPage4 As TabPage
    Friend WithEvents DataToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents NyquistCurveToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeviationsPlotToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RLKKPlotToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DataToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents RLKKParametersToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RLKKParametersToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents OpenFileDialog2 As OpenFileDialog
    Friend WithEvents SaveFileDialog2 As SaveFileDialog
    Friend WithEvents Button2 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Button3 As Button
End Class
