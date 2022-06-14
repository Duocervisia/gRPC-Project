from concurrent import futures

import grpc
from plotter import Plotter
import waterlevel_pb2_grpc
import waterlevel_pb2


class WaterLevelServicer(waterlevel_pb2_grpc.WaterLevelServicer):

    def GetData(self, request_iterator, context):

        print("Client connected with IP: ",context.peer())

        plotter = Plotter()
        jsonString = plotter.getJson(request_iterator.beginning_timestamp)

        return waterlevel_pb2.GetDataReply(
            json=jsonString
        )


    def GetServerResponse(self, request_iterator, context):
        for message in request_iterator:
            yield message


def serve():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    waterlevel_pb2_grpc.add_WaterLevelServicer_to_server(WaterLevelServicer(), server)
    server.add_insecure_port('[::]:50051')
    server.start()
    server.wait_for_termination()


if __name__ == '__main__':
    print("running\n")
    serve()
