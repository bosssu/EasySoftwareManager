
cd Tools\ExcelToProto\Table
python xls_deploy_tool.py

copy *.bytes ..\..\..\Assets\Resources\table_data\
copy *.proto ..\out\proto\
copy AllTableProto.cs ..\..\..\Assets\Scripts\DataManager\
copy TableProtoLoader.cs ..\..\..\Assets\Scripts\DataManager\

pause