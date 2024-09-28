# ������ ������������� � Ocelot

## ��������

������ ������ ������� �� ���������� �������������, ������� ��������������� ����� API Gateway Ocelot. 

## ����������

- Docker
- Docker Compose

## ���������

1. ���������� �����������:
https://github.com/ilara90.git

2. ���������, ��� � ��� ����������� Docker � Docker Compose. �� ������ ��������� ��� � ������� ������:
bash
   docker --version
   docker-compose --version


## ������ �������

1. �������� � ��������� ��� ������� � ������� Docker Compose:
bash
   docker-compose up --build

2. ����� ��������� �������, ��� ������� ����� �������� �� ��������� �������:
   - ApiGateway: http://localhost:10758
   - OrderService: http://localhost:34182

## ������ ������� � API ��� �������� ������

����� ������� ������� �� ������ ��������� ������ �� �������� ������ ����� Ocelot. ��������, ��������� curl:
bash
curl -X POST http://localhost:10758/api/orders \\
-H "Content-Type: application/json" \\
-d '
"email": "test@gmail.com",
{
  "Id": "123",
  "productName": "pizza"
  "quantity": 2
}'

### ��������� �������:
- email: ���������� �����, ���� ��������� ��������� �����.
- Id: ID ��������, ������� �� ������ ��������.
- productName: �������� ��������, ������� �� ������ ��������
- quantity: ���������� ������.

### �����:
��� �������� �������� ������ �� �������� ����� � ����������� � ������, ��������:
Send email to: "test@gmail.com", order: "Id": "123", "productName": "pizza", "quantity": 2.

## ��������� �������
����� ���������� ��� �������, ������� Ctrl + C � ���������, ��� ������� Docker Compose, ��� ��������� ��������� ������� � ������ ���������:
bash
docker-compose down