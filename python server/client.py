from __future__ import print_function

import grpc
import waterlevel_pb2_grpc
import waterlevel_pb2


def make_message():
    return waterlevel_pb2.GetDataRequest(
        beginning_timestamp=1655120774
    )


def send_message(stub):
    response = stub.GetData(make_message())
    print(response.json)


def run():
    with grpc.insecure_channel('localhost:50051') as channel:
        stub = waterlevel_pb2_grpc.WaterLevelStub(channel)
        send_message(stub)


if __name__ == '__main__':
    run()
