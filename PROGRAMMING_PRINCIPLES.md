## Тип архітектури: REST API (Monolithic)

[Підключено Microsoft.AspNetCore.OpenApi](https://github.com/wghtl4s/Malwine/blob/bb119a42a85769dc78ab5eb42acfff2363cf5cfd/Malwine/Malwine.csproj#L12)
Ці бібліотеки генерують Swagger (документацію для API).


## Single Responsibility Principle (SRP - Принцип єдиної відповідальності)

Кожен клас або метод має виконувати лише одну задачу.
Контролер MoviesController займається виключно фільмами. [Методи](https://github.com/wghtl4s/Malwine/blob/bb119a42a85769dc78ab5eb42acfff2363cf5cfd/Malwine/Controllers/MoviesController.cs#L162-L176) (Наприклад, Like для вподобайки) не втручаються в роботу один одного. 


## Eager Loading
Користувач і разом з ним  всі фільми, які він лайкнув. Це робиться [одним запитом (JOIN).](https://github.com/wghtl4s/Malwine/blob/bb119a42a85769dc78ab5eb42acfff2363cf5cfd/Malwine/Controllers/UsersController.cs#L28C40-L29C6)

## DTO (Data Transfer Object)
[Створення нового анонімного об'єкта](https://github.com/wghtl4s/Malwine/blob/bb119a42a85769dc78ab5eb42acfff2363cf5cfd/Malwine/Controllers/UsersController.cs#L34-L47) var userDto = new { ... }


## Fail Fast (Швидка відмова)
[Перевірка наявності даних на самому початку](https://github.com/wghtl4s/Malwine/blob/bb119a42a85769dc78ab5eb42acfff2363cf5cfd/Malwine/Controllers/UsersController.cs#L32). Якщо їх немає, то одразу переривається виконання методу. 
