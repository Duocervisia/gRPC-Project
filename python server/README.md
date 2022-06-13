# Python gRPC

Introduction: https://www.velotio.com/engineering-blog/grpc-implementation-using-python

Save requirements: py -m pip freeze > requirements.txt
Install requirements: py -m pip install -r requirements.txt
Create stubs: py -m grpc_tools.protoc --proto_path=. ./waterlevel.proto --python_out=. --grpc_python_out=.

