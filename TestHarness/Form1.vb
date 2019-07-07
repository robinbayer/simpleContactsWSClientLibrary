Public Class Form1

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        Dim objController As New Overthink.SimpleContactsWSClientLibrary.Controller()
        Dim objCallResult As Overthink.SimpleContactsWSClientLibrary.APICallResult


        Dim objContact = New Overthink.SimpleContactsWSClientLibrary.Contact()

        'Public string contactId { Get; Set; }
        'Public string firstName { Get; Set; }
        'Public string lastName { Get; Set; }
        'Public string cellPhone { Get; Set; }
        'Public string homePhone { Get; Set; }
        'Public string workPhone { Get; Set; }
        'Public string emailAddress { Get; Set; }
        'Public DateTime birthDate { Get; Set; }
        'Public string locationAddress { Get; Set; }

        objContact.contactId = "e35d4a39-4fa9-44e5-bd2d-a297643c62e4"
        objContact.firstName = "Robin"
        objContact.lastName = "Bayer"
        objContact.cellPhone = "913-645-6666"
        objContact.homePhone = "316-942-7019"
        objContact.workPhone = "785-371-4970"
        objContact.birthDate = #9/19/1970#
        objContact.locationAddress = "411 Fig Vine Drive"

        'objCallResult = objController.updateContact(objContact)

        objContact = objController.getContactById("e35d4a39-4fa9-44e5-bd2d-a297643c62e4")




        ' e35d4a39-4fa9-44e5-bd2d-a297643c62e4

        If (objCallResult.resultCode = Overthink.SimpleContactsWSClientLibrary.Controller.METHOD_RETURN_SUCCESS) Then

            MsgBox(objCallResult.successKeyValue)

        Else

            MsgBox(objCallResult.errors.Count)

        End If


    End Sub


End Class
