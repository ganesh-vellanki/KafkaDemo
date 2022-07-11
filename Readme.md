# Getting started
* Install docker desktop in your local machine.
* Run `docker-compose up -d` to setup zookeeper & kafka infrastructure.
* Build & run `KafkaDemo` C# application to see kafka producer & consumer in action.
* Note that, `AutoOffsetReset` as `AutoOffsetReset.Earliest` to read all past messages (or) `AutoOffsetReset.Latest` to read messages only when consumer boots up (latest) @ line no 76.
* Use docker desktop application to manage created infrastructure.