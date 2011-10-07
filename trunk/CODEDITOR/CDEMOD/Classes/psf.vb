Imports System.IO
Imports System.Text

Public Class psf

    Public Function GETID(ByVal filename As String) As String

        Dim fs As New FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read)
        Dim iso As String
        Dim bs(2047) As Byte

        If fs.Length > 2048 Then

            fs.Read(bs, 0, 2048)
            If bs(0) = &H0 AndAlso bs(1) = &H50 AndAlso bs(2) = &H42 AndAlso bs(3) = &H50 Then

                Dim md5 As System.Security.Cryptography.MD5 = _
                    System.Security.Cryptography.MD5.Create()
                'ハッシュ値を計算する 
                Dim b As Byte() = md5.ComputeHash(bs)

                'ファイルを閉じる 
                fs.Close()

                Dim hex(4) As UInteger
                Dim code(3) As Byte
                For i = 0 To 3
                    Array.ConstrainedCopy(b, i << 2, code, 0, 4)
                    hex(i) = BitConverter.ToUInt32(code, 0)
                Next
                hex(4) = hex(0) Xor hex(1)
                hex(4) = hex(4) Xor hex(2)
                hex(4) = hex(4) Xor hex(3)

                Dim result As String = "HB" & hex(4).ToString("X")

                Return result
            End If
        End If

        iso = imagecheck(fs.Length, fs)
        fs.Close()
        Return iso

    End Function


    Public Function GETNAME(ByVal filename As String) As String

        Dim fs As New FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read)
        Dim iso As String
        Dim bs(2047) As Byte

        If fs.Length > 2048 Then

            fs.Read(bs, 0, 2048)
            If bs(0) = &H0 AndAlso bs(1) = &H50 AndAlso bs(2) = &H42 AndAlso bs(3) = &H50 Then

                Dim fileoffset As Integer = CInt(bs(40 + 12))

                Dim z As Integer = fileoffset + 40
                Dim i As Integer = 0
                Dim k As Integer = 0
                While bs(68 + k * 16) <> &H80
                    k += 1
                    If k = 32 Then
                        Return ""
                    End If
                End While
                k = CInt(bs(72 + k * 16))
                z += k
                While bs(z + i) <> &H0 AndAlso i < 64
                    i += 1
                End While
                Dim name As Byte() = Nothing
                Array.Resize(name, i)
                Array.ConstrainedCopy(bs, z, name, 0, i)

                Dim result As String = Encoding.GetEncoding(65001).GetString(name)

                fs.Close()
                Return result
            End If
        End If
        iso = imagecheck(fs.Length, fs)
        fs.Close()
        Return iso
    End Function

    Function imagecheck(ByVal filelen As Int64, ByVal bs As FileStream) As String
        '32848 iso size
        Dim size(3) As Byte
        Dim isosize As Int32 = 0
        If filelen - 32848 > 2048 Then
            bs.Seek(32848, SeekOrigin.Begin)
            bs.Read(size, 0, 4)
            isosize = BitConverter.ToInt32(size, 0)
            isosize *= 2048
            If filelen = isosize Then
                Return "ISO"
            End If
        End If
        Return ""
    End Function

End Class
