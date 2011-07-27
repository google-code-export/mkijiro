
'ICOMPAREはインデックスを破壊する?のでつかわない
'Public Class GID_sort

'    Implements IComparer

'    Private Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare

'        Dim tx As TreeNode = CType(x, TreeNode)
'        Dim ty As TreeNode = CType(y, TreeNode)

'        If tx.Level = 1 And ty.Level = 1 Then ' If the treenode is level 1, aka a Game title
'            Return String.Compare(tx.Tag.ToString, ty.Tag.ToString) ' Sort the nodes by the game node tags which contain the GID's
'        Else
'            Return 0 ' If not, don't sort it.
'        End If

'    End Function

'End Class

'Public Class GID_sortz

'    Implements IComparer

'    Private Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare

'        Dim tx As TreeNode = CType(x, TreeNode)
'        Dim ty As TreeNode = CType(y, TreeNode)

'        If tx.Level = 1 And ty.Level = 1 Then ' If the treenode is level 1, aka a Game title
'            Return String.Compare(ty.Tag.ToString, tx.Tag.ToString) ' Sort the nodes by the game node tags which contain the GID's
'        Else
'            Return 0 ' If not, don't sort it.
'        End If

'    End Function

'End Class

'Public Class G_Title_sort

'    Implements IComparer

'    Private Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare

'        Dim tx As TreeNode = CType(x, TreeNode)
'        Dim ty As TreeNode = CType(y, TreeNode)

'        If tx.Level = 1 And ty.Level = 1 Then ' If the treenode is level 1, aka a Game title
'            Return String.Compare(tx.Text, ty.Text) ' Sort the nodes by the game node titles
'        Else
'            Return 0 ' If not, don't sort it.
'        End If

'    End Function

'End Class

'Public Class G_Title_sortz

'    Implements IComparer

'    Private Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare

'        Dim tx As TreeNode = CType(x, TreeNode)
'        Dim ty As TreeNode = CType(y, TreeNode)

'        If tx.Level = 1 And ty.Level = 1 Then ' If the treenode is level 1, aka a Game title
'            Return String.Compare(ty.Text, tx.Text) ' Sort the nodes by the game node titles
'        Else
'            Return 0 ' If not, don't sort it.
'        End If

'    End Function

'End Class

