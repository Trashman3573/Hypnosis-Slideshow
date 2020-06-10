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

    Dim imgType() As String = {"*.jpg", "*.png", "*.gif"}

    Dim mainList As List(Of String) = New List(Of String)
    Dim devList As List(Of String) = New List(Of String)
    Dim mainIndex As Integer
    Dim devIndex As Integer
    Dim deviation As Integer
    Dim devCount As Integer

    'Remember windows size/pos
    Dim preLoc As Point
    Dim preSize As Size
    Dim pbPreSize As Size

    'Public realod As Boolean = True
    Dim FullscreenModeBool As Boolean = False
    Dim Paused As Boolean = True
    Dim screen As Screen

    Private InitialStyle As Integer
    Dim PercentVisible As Decimal

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Init()

    End Sub

    Private Sub Init()
        Button2.Enabled = False
        deviation = My.Settings.deviation
        devCount = deviation
        Randomize()
        If My.Settings.mainDirectory <> "" Then
            Try
                ListInit(mainList, My.Settings.mainDirectory)
                If My.Settings.brainwashMode = True Then
                    ListInit(devList, My.Settings.altDirectory)
                End If
            Catch ex As Exception
            End Try
        End If


        If My.Settings.brainwashMode = True Then
            If devList.Count > 0 Then
                listShuffle(True)
                listShuffle(False)
                Button2.Enabled = True
                Label1.Text = mainIndex + 1.ToString & " / " & mainList.Count + 1
                PictureBox1.ImageLocation = mainList.Item(mainIndex)

            End If
        ElseIf mainList.Count > 0 Then
            listShuffle(True)
            Button2.Enabled = True
            Label1.Text = mainIndex + 1.ToString & " / " & mainList.Count + 1
            PictureBox1.ImageLocation = mainList.Item(mainIndex)

        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form2.Show()
        Me.Enabled = False
    End Sub

    Private Sub ListInit(alist As List(Of String), path As String)
        alist.Clear()
        For Each ext As String In imgType
            Dim a As String() = Directory.GetFiles(path, ext, SearchOption.AllDirectories)
            For Each img As String In a
                alist.Add(img)
            Next
        Next
    End Sub

    Private Sub listShuffle(main As Boolean)
        Randomize()
        Dim temp As List(Of String) = New List(Of String)
        If main = True Then
            For Each img As String In mainList
                temp.Add(img)
            Next
            mainList.Clear()
            mainIndex = 0
            For i = 0 To temp.Count - 1 Step 1
                Randomize()
                Dim j As Integer = CInt(Math.Floor((temp.Count() - 1 - 0 + 1) * Rnd())) + 0
                mainList.Add(temp.Item(j).ToString)
                temp.RemoveAt(j)
            Next
        Else
            For Each img As String In devList
                temp.Add(img)
            Next
            devList.Clear()
            devIndex = 0
            devCount = deviation
            For i = 0 To temp.Count - 1 Step 1
                Randomize()
                Dim j As Integer = CInt(Math.Floor((temp.Count() - 1 - 0 + 1) * Rnd())) + 0
                devList.Add(temp.Item(j).ToString)
                temp.RemoveAt(j)
            Next
        End If
    End Sub

    Private Sub Slide()
        If My.Settings.brainwashMode = True Then
            devCount -= 1
            If devCount = 0 Then
                devCount = deviation
                If devIndex = devList.Count - 1 Then
                    listShuffle(False)
                Else
                    devIndex += 1
                End If
                PictureBox1.ImageLocation = devList.Item(devIndex)
            Else
                If mainIndex = mainList.Count - 1 Then
                    listShuffle(True)
                Else
                    mainIndex += 1
                End If
                PictureBox1.ImageLocation = mainList.Item(mainIndex)
                Label1.Text = mainIndex + 1.ToString & " / " & mainList.Count
            End If
        Else
            If mainIndex = mainList.Count - 1 Then
                listShuffle(True)
            Else
                mainIndex += 1
            End If
            PictureBox1.ImageLocation = mainList.Item(mainIndex)
            Label1.Text = mainIndex + 1.ToString & " / " & mainList.Count
        End If
    End Sub

    Private Sub FullScreenMode(state As Boolean)
        screen = Screen.FromControl(Me)
        If state = True Then

            preLoc = Me.Location
            preSize = Me.Size
            pbPreSize = PictureBox1.Size

            Button1.Hide()
            Button2.Hide()
            Label1.Hide()

            Me.FormBorderStyle = FormBorderStyle.None
            Me.BackColor = Color.Black
            Me.Size = New Size(screen.Bounds.Width, screen.Bounds.Height)
            Me.Location = New Point(screen.Bounds.X, screen.Bounds.Y)

            PictureBox1.BorderStyle = BorderStyle.None
            PictureBox1.Location = New Point(0, 0)
            PictureBox1.Size = New Size(screen.Bounds.Width, screen.Bounds.Height)

            If My.Settings.transparency > 0 Then
                InitialStyle = GetWindowLong(Me.Handle, -20)
                PercentVisible = My.Settings.transparency / 100
                SetWindowLong(Me.Handle, -20, InitialStyle Or &H80000 Or &H20)
                SetLayeredWindowAttributes(Me.Handle, 0, 255 * PercentVisible, &H2)
                Me.TopMost = True
            End If
            FullscreenModeBool = True
        ElseIf state = False Then
            Button1.Show()
            Button2.Show()
            Label1.Show()

            PictureBox1.BorderStyle = BorderStyle.FixedSingle

            Me.FormBorderStyle = FormBorderStyle.FixedSingle
            Me.BackColor = SystemColors.Control
            Me.Size = preSize
            Me.Location = preLoc
            Me.TopMost = False

            PictureBox1.Location = New Point(5, 5)
            PictureBox1.Size = pbPreSize
            FullscreenModeBool = False
        End If
    End Sub
    Private Sub playToggle(action As Boolean)
        If action = False Then
            Timer1.Stop()
            Timer2.Stop()
            Paused = True
        ElseIf action = True Then
            Timer1.Interval = My.Settings.speed
            Timer2.Interval = My.Settings.deviationInterval * 60 * 1000
            Timer1.Start()
            Timer2.Start()
            Paused = False
        End If
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        devCount -= 1
        FullScreenMode(True)
        playToggle(True)
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Slide()
    End Sub
    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        If deviation > My.Settings.maxDeviation Then
            deviation -= 1
            Console.WriteLine(deviation)
        ElseIf deviation = My.Settings.maxDeviation Then
            Timer2.Stop()

        End If
    End Sub
    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If FullscreenModeBool = True Then
                FullScreenMode(False)
                playToggle(False)
            End If
        End If
    End Sub

    Private Sub Form1_EnabledChanged(sender As Object, e As EventArgs) Handles MyBase.EnabledChanged
        If Me.Enabled = True Then
            If My.Settings.mainDirectory <> "" Then
                ListInit(mainList, My.Settings.mainDirectory)
                If My.Settings.brainwashMode = True And My.Settings.altDirectory <> "" Then
                    ListInit(devList, My.Settings.altDirectory)
                End If
            End If
            My.Settings.Reload()
            deviation = My.Settings.deviation
            Init()
        End If

    End Sub


End Class
