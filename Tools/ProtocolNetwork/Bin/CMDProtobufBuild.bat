cd Tools\ProtocolNetwork\Bin

del Cmd.cs

del Cmd.dll

del CmdSerializer.dll

del Cmd.bin

del Cmd.proto

rd /s /q temp

md temp

python removepack.py .. temp

type temp\*.proto >> Cmd.proto

findstr /iv "import" Cmd.proto>Cmd1.proto&move Cmd1.proto Cmd.proto

findstr /iv "package" Cmd.proto>Cmd1.proto&move Cmd1.proto Cmd.proto

copy /y Cmd.proto protobuf-net\ProtoGen

protobuf-net\ProtoGen\protoc -I. Cmd.proto -o Cmd.bin

protobuf-net\ProtoGen\protogen -i:Cmd.bin -o:Cmd.cs

copy Cmd.cs ..\..\..\Assets\Scripts\DataManager\

pause