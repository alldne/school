let fac x =
    if x = 1
    then 1
    else x * fac (x - 1) end

writeLine (fac 5)