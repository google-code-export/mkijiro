Imports System.IO
Imports System.Text
Imports System.Security.Cryptography

Public Class psf

    Public Function video(ByVal filename As String) As String

        Dim fs As New FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read)
        Dim bs(2047) As Byte
        Dim result As String = ""
        Dim m As umdisomanger = umdisomanger
        Dim MODE As String = ""

        If fs.Length > 2048 + 40 Then
            fs.Read(bs, 0, 2048)
            'PBP
            If bs(0) = &H0 AndAlso bs(1) = &H50 AndAlso bs(2) = &H42 AndAlso bs(3) = &H50 Then
                Return "PBP"
            End If
        End If


        If fs.Length > &H8060 AndAlso m.psx = False AndAlso result = "" Then
            fs.Seek(&H8000, SeekOrigin.Begin)
            fs.Read(bs, 0, 2048)
            '.CD001
            If bs(0) = &H1 AndAlso bs(1) = &H43 AndAlso bs(2) = &H44 AndAlso bs(3) = &H30 _
                AndAlso bs(4) = &H30 AndAlso bs(5) = &H31 Then
                'PSP GAME
                If bs(8) = &H50 AndAlso bs(9) = &H53 AndAlso bs(10) = &H50 AndAlso bs(11) = &H20 _
                    AndAlso bs(12) = &H47 AndAlso bs(13) = &H41 AndAlso bs(14) = &H4D AndAlso bs(15) = &H45 Then


                    fs.Close()
                    Return "PSP"

                ElseIf bs(8) = &H55 AndAlso bs(9) = &H4D AndAlso bs(10) = &H44 AndAlso bs(11) = &H20 _
                        AndAlso bs(12) = &H56 AndAlso bs(13) = &H49 AndAlso bs(14) = &H44 AndAlso bs(15) = &H45 AndAlso bs(16) = &H4F Then

                    fs.Close()
                    Return "VIDEO"
                End If
            End If
        End If

        Return ""

    End Function

    Public Function GETID(ByVal filename As String) As String

        Dim fs As New FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read)
        Dim bs(2047) As Byte
        Dim result As String = ""
        Dim m As umdisomanger = umdisomanger
        Dim NOID As Boolean = False

        If fs.Length > 2048 + 40 Then
            fs.Read(bs, 0, 2048)
            'PBP
            If bs(0) = &H0 AndAlso bs(1) = &H50 AndAlso bs(2) = &H42 AndAlso bs(3) = &H50 Then

                fs.Seek(40, SeekOrigin.Begin)
                fs.Read(bs, 0, 2048)

                'PSF
                If bs(0) = &H0 AndAlso bs(1) = &H50 AndAlso bs(2) = &H53 AndAlso bs(3) = &H46 Then

                    Dim offset(3) As Byte
                    Dim ms As Boolean = False
                    Array.ConstrainedCopy(bs, 8, offset, 0, 4)
                    Dim i As Integer = BitConverter.ToInt32(offset, 0)
                    Array.ConstrainedCopy(bs, 12, offset, 0, 4)
                    Dim k As Integer = BitConverter.ToInt32(offset, 0)
                    Array.ConstrainedCopy(bs, 16, offset, 0, 4)
                    Dim z As Integer = BitConverter.ToInt32(offset, 0)
                    Dim psfbyte(200) As Byte
                    Array.ConstrainedCopy(bs, i, psfbyte, 0, 200)
                    Dim pname As String = Encoding.GetEncoding(65001).GetString(psfbyte)
                    pname = pname.Substring(0, pname.IndexOf(Chr(0) & Chr(0)))
                    Dim psfst As String() = pname.Split(Chr(0))
                    For i = 0 To z - 1
                        If psfst(i) = "DISC_ID" Then
                            Exit For
                        End If
                        If psfst(i) = "UPDATER_VER" Then
                            ms = True
                        End If
                    Next
                    If i = z Then
                        NOID = True
                    End If

                    Dim gidpsf(8) As Byte
                    k += CInt(bs(32 + 16 * i))
                    Array.ConstrainedCopy(bs, k, gidpsf, 0, 9)
                    result = Encoding.GetEncoding(0).GetString(gidpsf)
                    If result.Contains("MSTK") Then
                        ms = True
                    End If

                    If result = "UCJS10041" Or NOID = True Or My.Settings.hbhash = True Or ms = True Then
                        fs.Seek(0, SeekOrigin.Begin)
                        fs.Read(bs, 0, 2048)
                        Dim md5 As MD5 = md5.Create()
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

                        result = "HB" & hex(4).ToString("X")


                        If ms = True Then
                            result = "UP" & result.Remove(0, 2)
                        End If

                        Return result
                    Else
                        If m.psx = True Then
                            result = result.Insert(4, "_")
                        Else
                            result = result.Insert(4, "-")
                        End If
                        fs.Close()
                        Return result
                    End If
                End If
                'DAX
            ElseIf bs(0) = &H44 AndAlso bs(1) = &H41 AndAlso bs(2) = &H58 AndAlso bs(3) = &H0 Then
                result = ""
                'CISO
            ElseIf bs(0) = &H43 AndAlso bs(1) = &H49 AndAlso bs(2) = &H53 AndAlso bs(3) = &H4F Then
                result = ""
                'JISO
            ElseIf bs(0) = &H4A AndAlso bs(1) = &H49 AndAlso bs(2) = &H53 AndAlso bs(3) = &H4F Then
                result = ""
            End If

            If fs.Length > &H8060 AndAlso m.psx = False AndAlso result = "" Then
                fs.Seek(&H8000, SeekOrigin.Begin)
                fs.Read(bs, 0, 2048)
                '.CD001
                If bs(0) = &H1 AndAlso bs(1) = &H43 AndAlso bs(2) = &H44 AndAlso bs(3) = &H30 _
                    AndAlso bs(4) = &H30 AndAlso bs(5) = &H31 Then
                    'PSP GAME
                    If bs(8) = &H50 AndAlso bs(9) = &H53 AndAlso bs(10) = &H50 AndAlso bs(11) = &H20 _
                        AndAlso bs(12) = &H47 AndAlso bs(13) = &H41 AndAlso bs(14) = &H4D AndAlso bs(15) = &H45 Then

                        Dim gid(9) As Byte

                        Array.ConstrainedCopy(bs, &H373, gid, 0, 10)
                        result = Encoding.GetEncoding(0).GetString(gid)

                        fs.Close()
                        Return result

                        'UMDVIDEO
                    ElseIf bs(8) = &H55 AndAlso bs(9) = &H4D AndAlso bs(10) = &H44 AndAlso bs(11) = &H20 _
                            AndAlso bs(12) = &H56 AndAlso bs(13) = &H49 AndAlso bs(14) = &H44 AndAlso bs(15) = &H45 AndAlso bs(16) = &H4F Then
                        Dim gid(9) As Byte

                        Array.ConstrainedCopy(bs, &H373, gid, 0, 10)
                        result = Encoding.GetEncoding(0).GetString(gid)

                        fs.Close()
                        Return result
                    End If
                End If
            End If
        End If

        fs.Close()
        Return result

    End Function


    Public Function GETNAME(ByVal filename As String, ByVal mode As String) As String

        Dim fs As New FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read)
        Dim bs(2047) As Byte
        Dim result As String = ""
        Dim m As umdisomanger = umdisomanger

        If fs.Length > 2048 + 40 Then

            fs.Read(bs, 0, 2048)
            'PBP
            If bs(0) = &H0 AndAlso bs(1) = &H50 AndAlso bs(2) = &H42 AndAlso bs(3) = &H50 Then

                fs.Seek(40, SeekOrigin.Begin)
                fs.Read(bs, 0, 2048)

                'PSF
                If bs(0) = &H0 AndAlso bs(1) = &H50 AndAlso bs(2) = &H53 AndAlso bs(3) = &H46 Then
                    Dim offset(3) As Byte
                    Array.ConstrainedCopy(bs, 8, offset, 0, 4)
                    Dim i As Integer = BitConverter.ToInt32(offset, 0)
                    Array.ConstrainedCopy(bs, 12, offset, 0, 4)
                    Dim k As Integer = BitConverter.ToInt32(offset, 0)
                    Array.ConstrainedCopy(bs, 16, offset, 0, 4)
                    Dim z As Integer = BitConverter.ToInt32(offset, 0)
                    Dim psfbyte(200) As Byte
                    Array.ConstrainedCopy(bs, i, psfbyte, 0, 200)
                    Dim pname As String = Encoding.GetEncoding(65001).GetString(psfbyte)
                    pname = pname.Substring(0, pname.IndexOf(Chr(0) & Chr(0)))
                    Dim psfst As String() = pname.Split(Chr(0))
                    i = 0
                    While True
                        If mode = "N" AndAlso psfst(i) = "TITLE" Then
                            Exit While
                        End If
                        If mode = "I" AndAlso psfst(i) = "DISC_VERSION" Then
                            Exit While
                        End If
                        If i = z Then
                            fs.Close()
                            Return ""
                        End If
                        i += 1
                    End While

                    Array.ConstrainedCopy(bs, 32 + i * 16, offset, 0, 4)
                    i = BitConverter.ToInt32(offset, 0)
                    k += i
                    Dim name(128) As Byte
                    Array.ConstrainedCopy(bs, k, name, 0, 128)
                    result = Encoding.GetEncoding(65001).GetString(name)
                    i = result.IndexOf(vbNullChar)
                    result = result.Substring(0, i)
                    If result = "" Then
                        result = ""
                    End If
                    fs.Close()
                    Return result
                Else
                    fs.Close()
                    Return ""
                End If
                'DAX
            ElseIf bs(0) = &H44 AndAlso bs(1) = &H41 AndAlso bs(2) = &H58 AndAlso bs(3) = &H0 Then
                result = ""
                'CISO
            ElseIf bs(0) = &H43 AndAlso bs(1) = &H49 AndAlso bs(2) = &H53 AndAlso bs(3) = &H4F Then
                result = ""
                'JISO
            ElseIf bs(0) = &H4A AndAlso bs(1) = &H49 AndAlso bs(2) = &H53 AndAlso bs(3) = &H4F Then
                result = ""
            End If

            If fs.Length > &H8060 AndAlso m.psx = False AndAlso result = "" Then
                fs.Seek(&H8000, SeekOrigin.Begin)
                fs.Read(bs, 0, 2048)
                '.CD001
                If bs(0) = &H1 AndAlso bs(1) = &H43 AndAlso bs(2) = &H44 AndAlso bs(3) = &H30 _
                    AndAlso bs(4) = &H30 AndAlso bs(5) = &H31 Then
                    'PSP GAME
                    'If bs(8) = &H50 AndAlso bs(9) = &H53 AndAlso bs(10) = &H50 AndAlso bs(11) = &H20 _
                    '    AndAlso bs(12) = &H47 AndAlso bs(13) = &H41 AndAlso bs(14) = &H4D AndAlso bs(15) = &H45 Then

                    Dim lba(3) As Byte
                    Dim i As Integer = 0
                    Dim k As Integer = 0
                    Dim z As Integer = 0
                    Dim size(7) As Byte
                    Dim str(8) As Byte

                    fs.Seek(&H8008, SeekOrigin.Begin)
                    fs.Read(str, 0, 9)
                    Dim iso As String = Encoding.GetEncoding(0).GetString(str)

                    fs.Seek(&H8050, SeekOrigin.Begin)
                    fs.Read(size, 0, 5)
                    Dim lbatotal As Int64 = BitConverter.ToInt64(size, 0)
                    lbatotal *= 2048
                    If lbatotal - fs.Length <= 2048 AndAlso (iso.Contains("PSP GAME") Or iso.Contains("UMD VIDEO")) Then

                        'PSP_GAME,UMD_VIDEO,LPATHTABLE
                        fs.Seek(&H808C, SeekOrigin.Begin) '0x809E rootdir
                        fs.Read(lba, 0, 2)
                        z = BitConverter.ToInt32(lba, 0)
                        If z * 2048 > fs.Length Then
                            fs.Close()
                            Return ""
                        End If
                        fs.Seek(z * 2048, SeekOrigin.Begin)
                        fs.Read(bs, 0, 2048)
                        i = 6
                        While True
                            'PSP_GAME
                            If iso.Contains("PSP GAME") = True AndAlso bs(i) = &H50 AndAlso bs(i + 1) = &H53 AndAlso bs(i + 2) = &H50 Then
                                Exit While
                            ElseIf iso.Contains("UMD VIDEO") = True AndAlso bs(i) = &H55 AndAlso bs(i + 1) = &H4D AndAlso bs(i + 2) = &H44 AndAlso bs(i + 3) = &H5F AndAlso bs(i + 4) = &H56 Then
                                Exit While
                            ElseIf i > 2038 Then
                                fs.Close()
                                Return ""
                            End If
                            i += 1
                        End While
                        'Array.ConstrainedCopy(bs, i - 31, lba, 0, 2) 'rootdir for 0x809E
                        Array.ConstrainedCopy(bs, i - 6, lba, 0, 2)
                        z = BitConverter.ToInt32(lba, 0)
                        If z * 2048 > fs.Length Then
                            fs.Close()
                            Return ""
                        End If
                        fs.Seek(z * 2048, SeekOrigin.Begin)
                        fs.Read(bs, 0, 2048)
                        i = 31
                        'PARAM.SFO
                        While True
                            If bs(i) = &H50 AndAlso bs(i + 1) = &H41 AndAlso bs(i + 2) = &H52 AndAlso bs(i + 3) = &H41 Then
                                Exit While
                            ElseIf i > 2038 Then
                                fs.Close()
                                Return ""
                            End If
                            i += 1
                        End While
                        Array.ConstrainedCopy(bs, i - 31, lba, 0, 3)
                        z = BitConverter.ToInt32(lba, 0)
                        If z * 2048 > fs.Length Then
                            fs.Close()
                            Return ""
                        End If
                        fs.Seek(z * 2048, SeekOrigin.Begin)
                        fs.Read(bs, 0, 2048)
                        'PSF
                        If bs(0) = &H0 AndAlso bs(1) = &H50 AndAlso bs(2) = &H53 AndAlso bs(3) = &H46 Then
                            Dim offset(3) As Byte
                            Array.ConstrainedCopy(bs, 8, offset, 0, 4)
                            i = BitConverter.ToInt32(offset, 0)
                            Array.ConstrainedCopy(bs, 12, offset, 0, 4)
                            k = BitConverter.ToInt32(offset, 0)
                            Array.ConstrainedCopy(bs, 16, offset, 0, 4)
                            z = BitConverter.ToInt32(offset, 0)
                            Dim psfbyte(200) As Byte
                            Array.ConstrainedCopy(bs, i, psfbyte, 0, 200)
                            Dim pname As String = Encoding.GetEncoding(65001).GetString(psfbyte)
                            pname = pname.Substring(0, pname.IndexOf(Chr(0) & Chr(0)))
                            Dim psfst As String() = pname.Split(Chr(0))
                            i = 0
                            While True
                                If mode = "N" AndAlso psfst(i) = "TITLE" Then
                                    Exit While
                                End If
                                If mode = "I" AndAlso psfst(i) = "DISC_VERSION" Then
                                    Exit While
                                End If
                                If i = z - 1 Then
                                    fs.Close()
                                    If mode <> "A" Then
                                        Return ""
                                    Else
                                        Exit While
                                    End If
                                End If
                                i += 1
                            End While

                            If mode = "A" Then
                                Dim name(128) As Byte
                                Dim ss As New StringBuilder
                                Dim h As Integer = 0
                                For i = 0 To z - 1
                                    Array.ConstrainedCopy(bs, 32 + i * 16, offset, 0, 4)
                                    h = BitConverter.ToInt32(offset, 0)
                                    Array.ConstrainedCopy(bs, k + h, name, 0, 128)
                                   If bs(k + h) < 20 Then
                                        Array.Resize(name, 4)
                                        result = BitConverter.ToInt32(name, 0).ToString
                                        Array.Resize(name, 128)
                                    Else
                                        result = Encoding.GetEncoding(65001).GetString(name)
                                        h = result.IndexOf(vbNullChar)
                                        result = result.Substring(0, h)
                                    End If
                                    ss.Append(psfst(i))
                                    ss.Append(" ")
                                    ss.AppendLine(result)
                                Next
                                result = ss.ToString
                            Else
                                Array.ConstrainedCopy(bs, 32 + i * 16, offset, 0, 4)
                                i = BitConverter.ToInt32(offset, 0)
                                k += i

                                Dim name(128) As Byte
                                Array.ConstrainedCopy(bs, k, name, 0, 128)
                                result = Encoding.GetEncoding(65001).GetString(name)
                                i = result.IndexOf(vbNullChar)
                                result = result.Substring(0, i)
                            End If

                            If result = "" Then
                                result = ""
                            End If

                            fs.Close()
                            Return result
                        Else
                            fs.Close()
                            Return ""
                        End If
                    End If

                End If
            End If
        End If

        fs.Close()
        Return result

    End Function

End Class
