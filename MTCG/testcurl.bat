

REM --------------------------------------------------
echo 2) Login Users
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
echo.
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"altenhof\", \"Password\":\"markus\"}"
echo.
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"admin\",    \"Password\":\"istrator\"}"
echo.

echo should fail:
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"different\"}"
echo.
echo.


curl -X POST http://localhost:10001/message --header "Content-Type: text/plain" -d "Hello World. I'm message number 2."
curl -X POST http://localhost:12000/message --header "Content-Type: text/plain" -d "Hello World. I'm message number 3."

rem Show first message
curl -X GET http://localhost:12000/message/1 --header "Content-Type: text/plain" -d ""

rem Show third message
curl -X GET http://localhost:12000/message/1 --header "Content-Type: text/plain" -d ""

rem Update a message
curl -X POST http://localhost:12000/message/1 --header "Content-Type: text/plain" -d "Hello World. I'm message number 1."

rem Delete a message
curl -X DELETE http://localhost:12000/message/1 --header "Content-Type: text/plain" -d ""

