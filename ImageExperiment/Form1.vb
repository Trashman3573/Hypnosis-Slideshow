Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions


Public Class Form1

    <DllImport("user32.dll", EntryPoint:="GetWindowLong")> Public Shared Function GetWindowLong(ByVal hWnd As IntPtr, ByVal nIndex As Integer) As Integer
    End Function
    <DllImport("user32.dll", EntryPoint:="SetWindowLong")> Public Shared Function SetWindowLong(ByVal hWnd As IntPtr, ByVal nIndex As Integer, ByVal dwNewLong As Integer) As Integer
    End Function
    <DllImport("user32.dll", EntryPoint:="SetLayeredWindowAttributes")> Public Shared Function SetLayeredWindowAttributes(ByVal hWnd As IntPtr, ByVal crKey As Integer, ByVal alpha As Byte, ByVal dwFlags As Integer) As Boolean
    End Function

    Dim Wdirectory As String
    Dim speed As Integer
    Dim transparency As Short
    Dim saved As Boolean = False

    Dim imgType() As String = {"*.jpg", "*.png", "*.gif"}
    Dim flatList As List(Of String) = New List(Of String)
    Dim imgList As List(Of String) = New List(Of String)

    Dim imgIndex As Integer = 0
    Dim Paused As Boolean = True

    Dim FullscreenModeBool As Boolean = False

    Dim screen As Screen

    Private InitialStyle As Integer
    Dim PercentVisible As Decimal

    Private Sub Init()
        Wdirectory = My.Settings.DeafultDir
        speed = My.Settings.DefaultSpeed
        transparency = My.Settings.DefaultTra
        Console.WriteLine(speed & "/" & transparency & "/" & Wdirectory)
        Button2.Enabled = False
        PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
        NumericUpDown2.Minimum = 1
        NumericUpDown2.Maximum = 100
        NumericUpDown2.Value = transparency
        NumericUpDown1.Value = speed

        Randomize()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Init()
        Try
            PicInit()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub PicInit()
        If Wdirectory <> "" Or Wdirectory <> " " Then
            ImgInit(Wdirectory)
            If flatList.Count > 0 Then

                listShuffle()
                Button2.Enabled = True
                PictureBox1.ImageLocation = imgList.Item(imgIndex)
                Label1.Text = imgIndex + 1.ToString & " / " & imgList.Count + 1
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        playToggle("stop")

        Dim dirBrowser As FolderBrowserDialog = New FolderBrowserDialog()
        dirBrowser.ShowDialog()
        If dirBrowser.SelectedPath <> "" Then
            Wdirectory = dirBrowser.SelectedPath
            My.Settings.DeafultDir = dirBrowser.SelectedPath
        End If

        PicInit()

    End Sub

    Private Sub ImgInit(path As String)

        flatList.Clear()
        For Each ext As String In imgType
            Dim a As String() = Directory.GetFiles(path, ext, SearchOption.AllDirectories)
            For Each img As String In a
                flatList.Add(img)
            Next
        Next
    End Sub

    Private Sub listShuffle()
        Dim temp As List(Of String) = New List(Of String)
        For Each img As String In flatList
            temp.Add(img)
        Next
        imgList.Clear()
        imgIndex = 0

        For i = 0 To temp.Count - 1 Step 1
            Randomize()
            Dim j As Integer = CInt(Math.Floor((temp.Count() - 1 - 0 + 1) * Rnd())) + 0
            imgList.Add(temp.Item(j).ToString)
            temp.RemoveAt(j)
        Next

    End Sub

    Dim preLoc As Point
    Dim preSize As Size
    Dim pPreSize As Size

    Private Sub Slide(direction As Boolean)

        If direction = True Then
            If imgIndex = imgList.Count - 1 Then
                listShuffle()
                imgIndex = 0
            Else
                imgIndex += 1
            End If

        ElseIf direction = False Then
            If imgIndex >= 1 Then
                imgIndex -= 1
            ElseIf imgIndex = 0 Then
                imgIndex = imgList.Count - 1
            End If
        End If
        PictureBox1.ImageLocation = imgList.Item(imgIndex)
        Label1.Text = imgIndex + 1.ToString & " / " & imgList.Count
    End Sub

    Private Sub FullScreenMode(state As Boolean)
        screen = Screen.FromControl(Me)
        Dim W As Integer = screen.Bounds.Width
        Dim H As Integer = screen.Bounds.Height
        If state = True Then


            preLoc = Me.Location
            preSize = Me.Size
            pPreSize = PictureBox1.Size

            Button1.Hide()
            Button2.Hide()

            PictureBox1.BorderStyle = BorderStyle.None
            NumericUpDown1.Hide()
            NumericUpDown2.Hide()
            Label1.Hide()
            CheckBox1.Hide()

            Me.FormBorderStyle = FormBorderStyle.None
            Me.BackColor = Color.Black
            Me.Size = New Size(W, H)
            Me.Location = New Point(screen.Bounds.X, screen.Bounds.Y)

            PictureBox1.Location = New Point(0, 0)
            PictureBox1.Size = New Size(W, H)

            If CheckBox1.Checked = True Then
                InitialStyle = GetWindowLong(Me.Handle, -20)
                PercentVisible = transparency / 100
                SetWindowLong(Me.Handle, -20, InitialStyle Or &H80000 Or &H20)
                SetLayeredWindowAttributes(Me.Handle, 0, 255 * PercentVisible, &H2)
                Me.TopMost = True
            End If
            FullscreenModeBool = True
        ElseIf state = False Then
            Button1.Show()
            Button2.Show()

            PictureBox1.BorderStyle = BorderStyle.FixedSingle
            NumericUpDown1.Show()
            NumericUpDown2.Show()
            Label1.Show()
            CheckBox1.Show()

            Me.FormBorderStyle = FormBorderStyle.FixedSingle
            Me.BackColor = SystemColors.Control
            Me.Size = preSize
            Me.Location = preLoc

            PictureBox1.Location = New Point(5, 5)
            PictureBox1.Size = pPreSize
            FullscreenModeBool = False
        End If
    End Sub

    Private Sub playToggle(Optional ByVal state As String = "toggle")
        If state = "stop" Then
            Timer1.Stop()
            Button2.Text = "Start"
            Paused = True
        ElseIf state = "start" Then
            speed = NumericUpDown1.Value
            transparency = NumericUpDown2.Value
            Timer1.Interval = speed
            Timer1.Start()
            Button2.Text = "Stop"
            Paused = False
        ElseIf state = "toggle" Then
            If Paused = True Then
                speed = NumericUpDown1.Value
                transparency = NumericUpDown2.Value
                Timer1.Interval = speed
                Timer1.Start()
                Button2.Text = "Stop"
                Paused = False
            Else
                Timer1.Stop()
                Button2.Text = "Start"
                Paused = True
            End If
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        FullScreenMode(True)
        playToggle("toggle")
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If imgIndex > imgList.Count - 1 Then
            listShuffle()
            Exit Sub
        End If
        Slide(True)
    End Sub

    Private Sub PictureBox1_DoubleClick(sender As Object, e As EventArgs) Handles PictureBox1.DoubleClick
        playToggle("stop")
        FullScreenMode(False)
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            CheckBox1.Text = "Transparency %:"
            NumericUpDown2.Enabled = True
        ElseIf CheckBox1.Checked = False Then
            CheckBox1.Text = "Transparent"
            NumericUpDown2.Enabled = False
        End If
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If imgList.Count > 0 Then
            If e.KeyCode = Keys.Left Then
                Slide(False)
            End If
            If e.KeyCode = Keys.Right Then
                Slide(True)
            End If
        End If
        If e.KeyCode = Keys.Escape Then
            If FullscreenModeBool = True Then
                FullScreenMode(False)
                playToggle("stop")
            End If
        End If
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        speed = NumericUpDown1.Value
        transparency = NumericUpDown2.Value
        My.Settings.DefaultTra = transparency
            My.Settings.DefaultSpeed = speed
            My.Settings.DeafultDir = Wdirectory
        My.Settings.Save()

    End Sub
End Class
