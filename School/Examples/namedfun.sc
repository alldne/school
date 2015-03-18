let product xs =
    fold xs 0 (fun acc x -> acc + x end) end

writeLine (product [1,2,3,4,5])
