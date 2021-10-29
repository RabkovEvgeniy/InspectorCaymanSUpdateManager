# Inspector Cayman S update manager

## Описание:
InspectorCaymanSUpdateManager - приложение позволяющее отслеживать и скачивать обновления для сигнатурного видеорегистратора с радар-детектором Inspector Cayman S

## Скриншоты:
![alt-текст](https://github.com/RabkovEvgeniy/InspectorCaymanSUpdateManager/blob/master/Inspector%20Cayman%20S%20update%20manager%20UI.png "UI основного окна")

## Принципы работы:
Все данные, включая: даты последних обновлений, ссылки на файлы обновлений и сами файлы обновлений, приложение берет с сайта https://www.rd-inspector.ru/,
где они представленны в открытом доступе.

## PS:
Для того, чтобы адаптировать программу под скачивания обновлений для другого устройства Inspector, достаточно реализовать:
- IMainwindowViewModelDataSource: интерфейс содержит два метода загружающих с сайта даты последних обновлений.
- IUpdateLoader: необходимо написать две реализации для скачивания и установки обновлений БД и ПО

Затем необходимо внедрить реализации через конструктор MainWindowViewModel в конструкторе MainWindow
