Imports System.Text
Imports System.IO

'ICOMPAREはインデックスを破壊する?のでつかわない
Class sort
    'さんぷるどおりだとうごくがなぜかうまくいかないので代替関数
    Public Function sort_game(ByVal mode As String) As Boolean
        Dim m As umdisomanger = umdisomanger

        m.TreeView1.BeginUpdate() ' This will stop the tree view from constantly drawing the changes while we sort the nodes

        Dim z As Integer = m.TreeView1.Nodes.Count
        Dim psf As New psf
        Dim i As Integer = 0
        Dim b1 As String = Nothing
        Dim b2 As String = Nothing
        Dim b3 As String = Nothing
        Dim s(z) As String
        Dim jp(z) As String
        Dim us(z) As String
        Dim eu(z) As String
        Dim hb(z) As String
        Dim c As Integer = 0
        Dim d As Integer = 0
        Dim e As Integer = 0
        Dim f As Integer = 0
        For Each n As TreeNode In m.TreeView1.Nodes
            If mode.Contains("GNAME") Then
                b1 = n.Text
            ElseIf mode.Contains("GID") Then
                b1 = n.Tag.ToString
            ElseIf mode.Contains("PSF") Then
                b1 = psf.GETNAME(n.Nodes(0).Tag.ToString, "N")
            ElseIf mode.Contains("FILE") Then
                b1 = Path.GetFileName(n.Nodes(0).Tag.ToString)
            End If
            Dim sb As New System.Text.StringBuilder()
            b2 = n.Index.ToString
            sb.Append(b1)
            sb.Append(" ,")
            sb.Append(b2)
            If mode.Contains("COUNTRY") Then
                If b1.Contains("J") AndAlso m.psx = False Then
                    jp(c) = sb.ToString
                    c += 1
                ElseIf b1.Contains("P") AndAlso m.psx = True Then
                    jp(c) = sb.ToString
                    c += 1
                ElseIf b1.Contains("US") Then
                    us(d) = sb.ToString
                    d += 1
                ElseIf b1.Contains("ES") Then
                    eu(e) = sb.ToString
                    e += 1
                ElseIf b1.Contains("HB") Then
                    hb(f) = sb.ToString
                    f += 1
                Else
                    s(i) = sb.ToString
                    i += 1
                End If
            Else
                s(i) = sb.ToString
                i += 1
            End If
        Next
        If mode.Contains("COUNTRY") Then
            Array.Resize(jp, c)
            Array.Resize(us, d)
            Array.Resize(eu, e)
            Array.Resize(hb, f)
            Array.Resize(s, i)
            Array.Sort(jp)
            Array.Sort(us)
            Array.Sort(eu)
            Array.Sort(hb)
            Array.Sort(s)
            Dim mergedArray As String()
            If mode.Contains("JP") Then
                mergedArray = jp.Union(us).ToArray()
                mergedArray = mergedArray.Union(eu).ToArray()
            ElseIf mode.Contains("US") Then
                mergedArray = us.Union(eu).ToArray()
                mergedArray = mergedArray.Union(jp).ToArray()
            Else
                mergedArray = eu.Union(us).ToArray()
                mergedArray = mergedArray.Union(jp).ToArray()
            End If
            mergedArray = mergedArray.Union(s).ToArray()
            mergedArray = mergedArray.Union(hb).ToArray()
            Array.Resize(s, z + 1)
            Array.Copy(mergedArray, 0, s, 1, z)
        Else
            Array.Sort(s)
        End If
            Dim j As Integer = 1
            Dim k As Integer = 0
            Dim y As Integer = 0
            Dim commaindex As Integer = 0
            Dim ss As String
            If mode.Contains("UP") Then
                While k < z
                    commaindex = s(j).LastIndexOf(",") + 1
                    ss = s(j).Substring(commaindex, s(j).Length - commaindex)
                    y = CInt(ss)
                    Dim cln As TreeNode = CType(m.TreeView1.Nodes(y).Clone(), TreeNode)
                    m.TreeView1.Nodes.Add(cln)
                    k += 1
                    j += 1
                End While
            ElseIf mode.Contains("DW") Then
                j = z
                While k < z
                    commaindex = s(j).LastIndexOf(",") + 1
                    ss = s(j).Substring(commaindex, s(j).Length - commaindex)
                    y = CInt(ss)
                    Dim cln As TreeNode = CType(m.TreeView1.Nodes(y).Clone(), TreeNode)
                    m.TreeView1.Nodes.Add(cln)
                    k += 1
                    j -= 1
                End While
            End If

            For p = 0 To z - 1
                m.TreeView1.Nodes.Remove(m.TreeView1.Nodes(0))
            Next
            m.TreeView1.Focus()
            m.TreeView1.EndUpdate() ' Update the changes made to the tree view.


            Return True
    End Function

End Class