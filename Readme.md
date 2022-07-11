# Getting started
* Install docker desktop in your computer.
* Run `docker-compose up -d` to setup zookeeper & kafka infrastructure.
* Build & run `KafkaDemo` C# application to see kafka producer & consumer in action.
* Note that, `AutoOffsetReset` as `AutoOffsetReset.Earliest` for reading all past messages (or) `AutoOffsetReset.Latest` for messages only when consumer boots up (latest) @ line no 76.
* Use docker desktop application to manage created infrastructure.