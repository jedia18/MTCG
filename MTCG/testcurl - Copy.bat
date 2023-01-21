
REM --------------------------------------------------
echo 3) create packages (done by "admin")
curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[{\"Id\":\"845f0dc7-37d0-426e-994e-43fc3ac83c08\", \"Name\":\"WaterGoblin\", \"Damage\": 10.0},{\"Id\":\"99f8f8dc-e25e-4a95-aa2c-782823f36e2a\", \"Name\":\"Dragon\", \"Damage\": 50.0}]"
echo.	