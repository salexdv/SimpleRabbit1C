# Простая COM-обертка RabbitMQ .NET Client для 1С

Поддерживает минимум методов, необходимых для отправки и получения сообщений

## Регистрация

```sh
.\src\reg.bat 
```

## Описание свойств

* **HostName** - *string*, адрес сервера
* **Port** - *integer*, порт сервера
* **VirtualHost** - *string*, имя виртуального хоста
* **UserName** - *string*, имя пользователя
* **Password** - *string*, пароль
* **QueueName** - *string*, имя очереди

## Описание методов

### CreateConnection

Устанавливает соединение с сервером

### CloseConnection

Закрывает текущее соединение

### QueueDeclare

Объявление очереди для отправки/получения сообщений

Параметры:
* **name** - *string*, имя очереди
* **durable** - *boolean*, признак сохраняемой на диск очереди

### SendMessage

Отправляет сообщение в объявленную ранее очередь

Параметры:
* **text** - *string*, текст сообщения

### ReceiveMessage

Получает сообщение из ранее объявленной очереди

Параметры:
* **autoAck** - *boolean*, признак автоматического подтверждения прочитанного сообщения
* **deliveryTag** - *ulong*, идентификатор сообщения, возвращаемый для ручного подтверждения

Возвращаемое значение:
* **text** - *string*, текст сообщения

### AckMessage

Подтверждение получения сообщения

Параметры:
* **deliveryTag** - *ulong*, идентификатор сообщения

### HasMessage

Проверка наличия сообщения сообщений в очереди

Возвращаемое значение:
* **true** - *boolean*, признак имеющегося сообщения в очереди

## Использование

## Создание соединения

```bsl
SimpleRabbit = Новый COMОбъект("AddIn.SimpleRabbit1C");
SimpleRabbit.HostName = "localhost";
SimpleRabbit.Port = 8672;
SimpleRabbit.VirtualHost = "/";
SimpleRabbit.UserName = "user";
SimpleRabbit.Password = "pass";

// Подключение к RabbitMQ
Попытка
    SimpleRabbit.CreateConnection()
Исключение
    ВызватьИсключение("Ошибка подключения серверу: " + КраткоеПредставлениеОшибки(ИнформацияОбОшибке()));
КонецПопытки;
```

### Отправка сообщения

```bsl
// Объявление очереди
SimpleRabbit.QueueDeclare("my_queue", Истина);
// Отправка сообщения
SimpleRabbit.SendMessage(Сообщение);
```

### Чтение сообщений

```bsl
// Объявление очереди
SimpleRabbit.QueueDeclare("my_queue", Истина);
Тег = 0;
Текст = SimpleRabbit.ReceiveMessage(Ложь, Тег);
Пока ЗначениеЗаполнено(Текст) Цикл
    ОбщегоНазначенияКлиентСервер.СообщитьПользователю(Текст);
    SimpleRabbit.AckMessage(Тег);
    Текст = SimpleRabbit.ReceiveMessage(Ложь, Тег);
КонецЦикла;
```

### Завершение работы

```bsl
// Отключение
SimpleRabbit.CloseConnection();
SimpleRabbit = Неопределено;
```

## Замеры производительности

Замер производился на локальном сервере c отправкой в качестве сообщения текущей универсальной даты в миллисекундах.
Скорость отправки: ~ 140000 сообщений/сек
Скорость получения с последующим подтверждением: ~ 1000 сообщений/сек
Скорость получения с автоматическим подтверждением: ~ 2000 сообщений/сек