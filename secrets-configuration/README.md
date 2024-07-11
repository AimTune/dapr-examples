```bash
# Secrets
docker-compose down
docker-compose up -d vault
docker exec -it secrets-configuration-vault-1 /bin/sh
vault login root
vault kv put secret/dapr/mysecret firstKey=aValue secondKey=anotherValue thirdKey=yetAnotherDistinctValue
exit
docker-compose up --build -d

# Configuration
docker exec secrets-configuration-redis-1 redis-cli MSET orderId1 "101" orderId2 "102"
docker exec secrets-configuration-redis-1 redis-cli MSET orderId1 "103" orderId2 "104"

docker exec -it secrets-configuration-vault-1 /bin/sh
vault login root
vault kv put secret/dapr/mysecret firstKey=dt secondKey=xtrb thirdKey=zeyrek
```

https://localhost:7204/get-secret
