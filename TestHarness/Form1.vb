Public Class Form1

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        Dim objController As New TequaCreek.eProcClientWSLibrary.Controller()
        Dim objCallResult As TequaCreek.eProcClientWSLibrary.GenericCallResult


        'Dim objPlacedInHoldForBudgetModificationEMailMessageParameter As New TequaCreek.eProcClientWSLibrary.PlacedInHoldForBudgetModificationEMailMessageParameter


        'objPlacedInHoldForBudgetModificationEMailMessageParameter.requisitionId = 201025
        'objPlacedInHoldForBudgetModificationEMailMessageParameter.specificReason = "This is the specific reason text for placing into Hold for Budget Modification queue"


        'objCallResult = objController.SendPlacedInBudgetHoldModificationEMailMessage(objPlacedInHoldForBudgetModificationEMailMessageParameter)


        'Dim objCallResult As List(Of TequaCreek.eProcClientWSLibrary.RequisitionApprovalHistoryEntry)
        'objCallResult = objController.AddApprovalHistoryEntry(59321, "super.visor", "robin.bayer")
        'For Each objApprovalHistoryEntry As TequaCreek.eProcClientWSLibrary.RequisitionApprovalHistoryEntry In objCallResult

        '    MsgBox(objApprovalHistoryEntry.SupervisorApprovedUserID)

        'Next

        'MsgBox(objCallResult.AdditionalInformation)

        'objController.SendDeniedRequisitionEMailMessage(64549, False, False, False, False)
        objCallResult = objController.PushEProcurementAttachmentsIntoOnBase(132667)
        'objCallResult = objController.PushPrePayAuthorizationDocumentIntoOnBase(201025)

        'If (objCallResult.ResultCode = TequaCreek.eProcClientWSLibrary.GenericCallResult.RESULT_CODE_SUCCESS) Then

        '    MsgBox("Success")

        'Else

        '    MsgBox(String.Format("Not successful - result is {0}, additional info is {1}", objCallResult.ResultCode, objCallResult.AdditionalInformation))

        'End If


        'objCallResult = objController.PushRequisitionAttachmentsIntoOnBase(132377)
        'objCallResult = objController.PushAttachmentsAsInvoiceIntoOnBase(131467)

        If (objCallResult.ResultCode = TequaCreek.eProcClientWSLibrary.GenericCallResult.RESULT_CODE_SUCCESS) Then

            MsgBox("Success")

        Else

            MsgBox(String.Format("Not successful - result is {0}, additional info is {1}", objCallResult.ResultCode, objCallResult.AdditionalInformation))

        End If



    End Sub


End Class
