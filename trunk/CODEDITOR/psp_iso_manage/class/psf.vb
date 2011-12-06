Imports System.IO
Imports System.Text
Imports System.Security.Cryptography

Public Class psf

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
                    Next
                    If i = z Then
                        NOID = True
                    End If

                    Dim gidpsf(8) As Byte
                    k += CInt(bs(32 + 16 * i))
                    Array.ConstrainedCopy(bs, k, gidpsf, 0, 9)
                    result = Encoding.GetEncoding(0).GetString(gidpsf)

                    If result = "UCJS10041" Or NOID = True Or My.Settings.hbhash = True Then
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

                        Return result
                    Else
                        If m.PSX = True Then
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

            If fs.Length > &H8060 AndAlso m.PSX = False AndAlso result = "" Then
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

                    ElseIf bs(8) = &H55 AndAlso bs(9) = &H4D AndAlso bs(10) = &H44 AndAlso bs(11) = &H20 _
                            AndAlso bs(12) = &H56 AndAlso bs(13) = &H49 AndAlso bs(14) = &H44 AndAlso bs(15) = &H45 AndAlso bs(16) = &H4F Then
                        fs.Close()
                        Return ""
                    End If
                End If
            End If
        End If

        fs.Close()
        Return result

    End Function


    Public Function GETNAME(ByVal filename As String) As String

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
                        If psfst(i) = "TITLE" Then
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

            If fs.Length > &H8060 AndAlso m.PSX = False AndAlso result = "" Then
                fs.Seek(&H8000, SeekOrigin.Begin)
                fs.Read(bs, 0, 2048)
                '.CD001
                If bs(0) = &H1 AndAlso bs(1) = &H43 AndAlso bs(2) = &H44 AndAlso bs(3) = &H30 _
                    AndAlso bs(4) = &H30 AndAlso bs(5) = &H31 Then
                    'PSP GAME
                    If bs(8) = &H50 AndAlso bs(9) = &H53 AndAlso bs(10) = &H50 AndAlso bs(11) = &H20 _
                        AndAlso bs(12) = &H47 AndAlso bs(13) = &H41 AndAlso bs(14) = &H4D AndAlso bs(15) = &H45 Then

                        Dim lba(3) As Byte
                        Dim i As Integer = 0
                        Dim k As Integer = 0
                        Dim z As Integer = 0
                        Dim size(7) As Byte

                        fs.Seek(&H8050, SeekOrigin.Begin)
                        fs.Read(size, 0, 5)
                        Dim lbatotal As Int64 = BitConverter.ToInt64(size, 0)
                        lbatotal *= 2048
                        If lbatotal - fs.Length <= 2048 Then

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
                            'PSP_GAME
                            While True
                                If bs(i) = &H50 AndAlso bs(i + 1) = &H53 AndAlso bs(i + 2) = &H50 Then
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
                                    If psfst(i) = "TITLE" Then
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
                        End If

                        'video
                    ElseIf bs(8) = &H55 AndAlso bs(9) = &H4D AndAlso bs(10) = &H44 AndAlso bs(11) = &H20 _
                            AndAlso bs(12) = &H56 AndAlso bs(13) = &H49 AndAlso bs(14) = &H44 AndAlso bs(15) = &H45 AndAlso bs(16) = &H4F Then
                        fs.Close()
                        Return ""
                    End If
                End If
            End If
        End If

        fs.Close()
        Return result

    End Function

End Class
