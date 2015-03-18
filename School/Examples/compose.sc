let addOne x = x + 1 end

let mulTwo y = y * 2 end

let duplicateThreeTimes z = [z, z, z] end

writeLine ((addOne >> mulTwo >> duplicateThreeTimes) 3)
