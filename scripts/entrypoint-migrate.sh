#!/bin/bash

set -e

until dotnet ef database update; do
  >&2 echo "PostgreSQL Server is starting up"
  sleep 1
done

>&2 echo "PostgreSQL Server is up - executing command"
exec "$@"
