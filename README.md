# Kool

Experimental compiler for the Kool language (a Kotlin-based language - thank you, JetBrains, I love your language and
yours IDEs, tools! S2) written in C# (which I love to use it too!
Thank you, Microsoft!).

> This project is for FUN, LEARNING, and EXPERIMENTING with the knowledge area of
> Compilers in Computer Science (except that I will start skipping the creation of
> the parser by myself because I really want to play with the generated stuff. And,
> that's is the reason why I converted the project from F# to C# again. Maybe in the
> future I do that! Maybe... :D).

## Goals

- [x] Investigate a parser to play with (using Irony for now);
- [x] Investigate how to parse and roughly generate the AST;
- [x] Investigate how to create partial tests to verify the generated AST;
- [ ] Finish to watch the videos from Immo Landwerth on YouTube about the creation of the Misky programming language.
  That should give me some ideas of how to create the .NET libraries;
- [ ] Define the Kool language;
- [ ] Create a Kool language parser;
- [ ] Transpile to JavaScript (or maybe TypeScript)?
- [ ] Transpile to C# (or maybe generate code directly with Roslyn)?

## The Kool language

Kool is almost the Kotlin language with a few changes in the syntax and, obviously, a smaller set of features. I'm
creating it for fun, I'm a father of a newborn baby (yes, I am not sleeping well by these days!), thus I don't have
enough time to create something better for now...

### Primitive Types

- [ ] `Char`
- [ ] `Int8`
- [ ] `Int16`
- [ ] `Int` = `Int32`
- [ ] `Int64`
- [ ] `Float`
- [ ] `Double`
- [ ] `Boolean`
- [ ] `String`

### Value declaration (non-mutable)

- `let x = 10`
- `let x: Int = 10`
- `let name = "Daniel"`
- `let finished = false`
- `let finished: Boolean = default`

### Variable declaration (mutable)

- `var x = 10`
- `var x: Int = 10`
- `var name = "Daniel"`
- `var finished = false`
- `var finished: Boolean = default`

### Expressions

* Example: sum of two numbers:

```
let x = 10
let y = 20
let sum = x + y
```

* Example: sum of two numbers using a single variable:

```
var sum = 0
sum = sum + 10
sum += sum + 20
```

#### Bit operators

* Binary operators

- `<< N` shift left N bits
- `>> N` shift right N bits
- `<~ N` rotate left N bits ? (IDEA)
- `~> N` rotate right N bits ? (IDEA)

#### Number Operators

* Unary operators

- `-` negates the number

* Binary operators

- `+` addition of two numbers
- `-` subtraction of two numbers
- `*` multiplication of two numbers
- `/` division of two numbers
- `%` remainder of the division between two numbers

#### Boolean Operators

- `!=` not equal
- `==` equal
- `<` less than
- `<=` less or equal than
- `>` greater than
- `>=` greater or equal than
- `!==` strict not equal (reference inequality) (KEEP?)
- `===` strict equality (reference equality) (KEEP?)

#### Logic Operators (used as text for readability)

- `not` logical NOT
- `and` logical AND 
- `or` logical OR
- `xor` logical XOR

### Control flow

#### Conditions

* if only

```
if value == true {
  // do something
}
```

* if / else

```
if value == true {
  // do something
} else {
  // or do this
}
```

* if / else if

```
if not goodBook {
  // discard it
} else if hasMore {
  // take another one!
}
```

* if / else if / else

```
if not goodBook {
  // discard it
} else if gift {
  // put it on the cabinet
} else {
  // throw it away
}
```

#### switch statement 

```
switch grade {
  case 'A':
    print("Superb!")
    
  case 'B':
    print("Very good!")
    
  case 'C', 'D':
    print("We need to study more...")
    
  default:
    print("Daddy is coming! Run!!") 
}
```

#### while statement

```
while not stack.empty {
  print("Next
}
```

#### for statement

```
for item in list {
}
```

#### if expressions

```
// ternary-like
let grade = if points >= 9 then 'A' else 'B'

// chained ifs
let grade = if points >= 9 then 'A' else if points >= 8 then 'B' else 'C'

// multi-line chained ifs
let grade =
  if points >= 9 then 'A'
  else if points >= 8 then 'B'
  else if points >= 7 then 'C'
  else 'D' 
```

#### when expression

```
let numberAsString = when digit {
  0 => "zero"
  1 => "one"
  2 => "two"
  3 => "three"
  else => "four"
}
```

## Samples

```
// complete form
func fibonacci(n: UInt): UInt {
  return 
    if n <= 2 then n
    else fibonacci(n - 1) + fibonacci(n - 2) 
}

// short form
func fibonacci(n: UInt): UInt =>
  if n <= 2 then n
  else fibonacci(n - 1) + fibonacci(n - 2)
```

```
func passByValue(value: Int) {
  value += 1
}

func passByRef(ref value: Int) {
  value += 1
}

var x = 10

passByValue(x)
print(x) // prints 10
passByRef(ref x)
print(x) // prints 11
```

```
func sum(array: Int[]): Int {
  var sum = 0
  
  for value in array {
    sum += value
  }
  
  return sum
}

func sum(array: Int[]): Int {
  return array.sum()
}
```

```
record class Point(
  let x: Int = 0
  let y: Int = 0
) {
  constructor(other: Point) 
    : this(other.x, other.y)
  
  let length: Int => Math.sqrt(x * x + y * y)
}

// same as
class Point(x: Int = 0, y: Int = 0) {
  private let _x: Int
  private let _y: Int
  
  init {
    _x = x
    _y = y
  }
  
  constructor(other: Point)
    : this(other.x, other.y)
    
  // full getter definition
  prop x: Int {
    get {
      return _x
    }
  }
  
  // simplified getter definition
  prop y: Int {
    get => _y
  }
  
  // fully-simplified getter-only definition
  prop length: Int => Math.sqrt(x * x + y * y)
  
  override func equals(other: Any): Boolean {
    return other is Point and x == other.x and y == other.y
  }
  
  override func getHashCode(): Int =>
    HashCode.Combine(x, y)
    
  open func deconstruct(): (Int, Int, Int) => (x, y, length)
}
```

> TODO Describe the language here

