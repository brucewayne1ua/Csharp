FROM ubuntu:latest
LABEL authors="netbook"

ENTRYPOINT ["top", "-b"]