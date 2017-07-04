# RxScans

This project is an experimentation (in its current state) into Rx
aggregations, like sum, count and others, where the observer is notified of
intermediate accumulator value _as_ the source elements are scanned.

To try the included demo, run the following commands from the root of the
project directory:

    $ dotnet restore
    $ dotnet run --project eg/RxScansDemo.csproj
