# Проект Микросервисов с Ocelot

## Описание

Данный проект состоит из нескольких микросервисов, которые взаимодействуют через API Gateway Ocelot. 

## Требования

- Docker
- Docker Compose

## Установка

1. Клонируйте репозиторий:
https://github.com/ilara90.git

2. Убедитесь, что у вас установлены Docker и Docker Compose. Вы можете проверить это с помощью команд:
bash
   docker --version
   docker-compose --version


## Запуск проекта

1. Соберите и запустите все сервисы с помощью Docker Compose:
bash
   docker-compose up --build

2. После успешного запуска, все сервисы будут доступны по следующим адресам:
   - ApiGateway: http://localhost:10758
   - OrderService: http://localhost:34182

## Пример запроса к API для создания заказа

После запуска проекта вы можете отправить запрос на создание заказа через Ocelot. Например, используя curl:
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

### Параметры запроса:
- email: Электроный адрес, куда требуется отправить заказ.
- Id: ID продукта, который вы хотите заказать.
- productName: Название продукта, который вы хотите заказать
- quantity: Количество товара.

### Ответ:
При успешном создании заказа вы получите ответ с информацией о заказе, например:
Send email to: "test@gmail.com", order: "Id": "123", "productName": "pizza", "quantity": 2.

## Остановка проекта
Чтобы остановить все сервисы, нажмите Ctrl + C в терминале, где запущен Docker Compose, или выполните следующую команду в другом терминале:
bash
docker-compose down