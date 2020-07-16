Imports System.ServiceModel.Description
Imports System.ServiceModel.Channels
Imports System.ServiceModel.Dispatcher

Public Class FaceB2BCustomEndpointBehavior
    'Inherits IEndpointBehavior

    Public Sub AddBindingParameters(ByVal endpoint As ServiceEndpoint, ByVal bindingParameters As System.ServiceModel.Channels.BindingParameterCollection)
    End Sub

    Public Sub ApplyClientBehavior(ByVal endpoint As ServiceEndpoint, ByVal clientRuntime As System.ServiceModel.Dispatcher.ClientRuntime)
        clientRuntime.MessageInspectors.Add(New FaceB2BCustomMessageInspector())
    End Sub

    Public Sub ApplyDispatchBehavior(ByVal endpoint As ServiceEndpoint, ByVal endpointDispatcher As System.ServiceModel.Dispatcher.EndpointDispatcher)
    End Sub

    Public Sub Validate(ByVal endpoint As ServiceEndpoint)
    End Sub
End Class

Public Class FaceB2BCustomMessageInspector
    'Inherits IClientMessageInspector
    'Implements IDispatchMessageInspector

    Public Sub AfterReceiveReply(ByRef reply As System.ServiceModel.Channels.Message, ByVal correlationState As Object)
    End Sub

    Public Function BeforeSendRequest(ByRef request As Message, ByVal channel As System.ServiceModel.IClientChannel) As Object
        Dim limit As Integer = request.Headers.Count

        For i As Integer = 0 To limit - 1

            If request.Headers(i).Name.Equals("VsDebuggerCausalityData") Then
                request.Headers.RemoveAt(i)
                Exit For
            End If
        Next

        Return request
    End Function

    Public Function AfterReceiveRequest(ByRef request As System.ServiceModel.Channels.Message, ByVal channel As System.ServiceModel.IClientChannel, ByVal instanceContext As System.ServiceModel.InstanceContext) As Object
        Return Nothing
    End Function

    Public Sub BeforeSendReply(ByRef reply As System.ServiceModel.Channels.Message, ByVal correlationState As Object)
    End Sub
End Class
