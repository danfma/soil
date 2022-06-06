# Soil

Playground to create a new language named Soil which should be compiled to 
.NET, JavaScript, and maybe others.

The language is hardly inspired by the Kotlin language, TypeScript, and C#.

> This project is for FUN, LEARNING, and EXPERIMENTING with the knowledge area of
> Compilers in Computer Science (except that I will start skipping the creation of
> the parser by myself because I really want to work with the generated stuff. And,
> that's is the reason why I converted the project from F# to C# again. Maybe in the
> future I do that! Maybe... :D).

## Goals

- [x] Investigate a parser to play with (using Irony for now);
- [x] Investigate how to parse and roughly generate the AST;
- [x] Investigate how to create partial tests to verify the generated AST;
- [ ] Create a semantic analyzer to verify the generated AST;
- [ ] Translate code to JavaScript;
- [ ] Translate code to C#;
- [ ] Translate code to .NET IL;
- [ ] Translate code to WebAssembly (maybe);
- [ ] Check other possible backend targets;

## The Soil language

Soil is almost the Kotlin language with a few changes in the syntax and, obviously, a smaller set of features. I'm
creating it for fun, I'm a father of a newborn baby (yes, I am not sleeping well by these days!), thus I don't have
enough time to create something better for now...

### Value declaration (non-reassignable variables)

- `val x = 10`
- `val x: Int = 10`
- `val name = "Daniel"`
- `val finished = false`
- `val finished: Bool = default`

### Variable declaration (mutable)

- `var x = 10`
- `var x: Int = 10`
- `var name = "Daniel"`
- `var finished = false`
- `var finished: Bool = default`

### Primitive Types

- [ ] `Char`
- [ ] `Int8`
- [ ] `Int16`
- [ ] `Int` = `Int32`
- [ ] `Int64`
- [ ] `Float`
- [ ] `Double`
- [ ] `Bool`
- [ ] `String`

### Special primitive types

- [ ] `Any` (any other type - the base type of all types)
- [ ] `Void` (no other type - the void, no value accepted)
- [ ] `Number` (base type for any type of number)
- [ ] `Range`
- [ ] `Index`

### Nullability

By default, not value accepts `null` in the language. You have to explicitly define it
as `nullable` to make that work.

For example:

```
val checked: Bool? = null
val enabled: Bool = null // it will show an error
```

### Expressions

* Example: sum of two numbers:

```
val x = 10
val y = 20
val sum = x + y
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
  print(">> ${stack.pop()}")
}
```

#### for-in statement

```
for item in list {
}
```

* simulating a for with a range and a jump to change the current index

```
for i in (0..10) {
  if i != 0 and i == 4 {
    jump 5
  }

  print(i)
}

// equivalent to the following javascript-code
for (val i = 0; i < 11; i += 1) {
  if (i !== 0 && i === 4) {
    i += 5;
    continue;
  }

  console.log('i', i)
}
```

#### if expressions

```
// ternary-like
val grade = if points >= 9 then 'A' else 'B'

// chained ifs
val grade = if points >= 9 then 'A' else if points >= 8 then 'B' else 'C'

// multi-line chained ifs
val grade =
  if points >= 9 then 'A'
  else if points >= 8 then 'B'
  else if points >= 7 then 'C'
  else 'D' 
```

#### when expression

```
val numberAsString = when digit {
  0 => "zero"
  1 => "one"
  2 => "two"
  3 => "three"
  else => "four"
}
```

## Namespaces

```
// per-file namespace
namespace My.Helpers

// use non-ambiguous namespace inferred from the referenced libraries
use Kool
// use non-ambiguous static class members 
use Kool.Math.*
// explicitly specify the assembly or package and then import its namespace
import "MyLib" use Some:Namespace

func sum(a: Int, b: Int): Int => a + b
func subtract(a: Int, b: Int): Int => a - b

record class Point(val x: Int, val y: Int) {
  prop length: Int => sqrt(x * x + y * y) as Int
}
```

## Classes

All classes are reference types by default.

```
class Person {
  // mutable fields (use val for readonly)
  var name: String = ""
  var surname: String? = null
  var age: Int = 0
  
  prop fullName: String => surname is not null ? "${name} ${surname}" : name
  
  func setName(name: String, surname: String, age: Int? = null) {
    this.name = name
    this.surname = surname
    
    if (age is not null) {
      this.age = age // automatic casting from Int? to Int
    }
  }
  
  override func toString(): String {
    return """Person { name = "${name}" }"""
  }
}

val person = Person()
person.setName("John", "Doe")
```

## Struct

All structs are value types by default.

```
struct Length(
  var value: Int = 0
)

func extend(length: Length): Length {
  length.value++
  return length
}

val source = Length()
val result = extend(source)

print(source.value) // prints 0
print(result.value) // prints 1
print(if source === result then "equal" else "not equal") // prints "not equal"
```

## Enum

```
// simple enum
enum ProcessStatus {
  New,
  Ready,
  Run,
  Terminated
}

var x = ProcessStatus.New
x = ProcessStatus.Ready

// enum with values
enum class LinkedListNode {
  Node(value: Int, next: LinkedListNode = None),
  None
}

var list = LinkedListNode.Node(1, LinkedListNode.Node(2))

print(list) // output: Node { value = 1, next = Node { value = 2, next = None } }

// enum as struct (value-type)
enum struct Pos {
  Top(value: Int),
  Bottom(value: Int)
}
```

## Generics

```
func sum<T>(a: T, b: T): Int
  where T extends Number {
  return a + b
}
```

## Samples

```
// compvale form
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

* Pass by value vs ref

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

* Closure

```
// shorter-version
func inc(increment: Int = 1) => (value: Int) => value + increment

// inner function inferred from outer type
func inc(increment: Int = 1): Func<(Int), Int> {
  return value => value + increment
}

// all typed
func inc(increment: Int = 1): Func<(value: Int), Int> {
  return (value: Int): Int => value + increment
}
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
  val x: Int = 0
  val y: Int = 0
) {
  constructor(other: Point) 
    : this(other.x, other.y)
  
  prop length: Int => Math.sqrt(x * x + y * y)
}

// same as
class Point(x: Int = 0, y: Int = 0) {
  private val _x: Int
  private val _y: Int
  
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
  
  override func equals(other: Any): Bool {
    return other is Point and x == other.x and y == other.y
  }
  
  override func getHashCode(): Int =>
    HashCode.Combine(x, y)
    
  open func deconstruct(): (Int, Int, Int) => (x, y, length)
}
```

// Possible React Integration through macro and protocols ???
```
protocol interface CounterProps for JsObject {
  val initialCount: Int? = null
}

val Counter = component!{ (props: JsObject with CounterProps): JsxElement => {
  val [count, setCount] = useState(props.initialCount ?? 0)
  
  val decrement = useCallback(
    { setCount({ it - 1 }) },
    []
  )
  
  val increment = useCallback(
    { setCount({ it + 1 }) },
    []
  )

  return jsx!{
    <div>
      <button type="button" onClick={decrement}> - </button>
      <span>{count}</span>
      </button> type="button" onClick={increment}> + </button>
    </div>
  }
}

// maybe using types and macros
[macro:FuncComponent]
class Counter {
  static operator func invoke(props: JsObject with CounterProps): JsxElement {
    val [count, setCount] = useState(props.initialCount ?? 0)
  
    val decrement = useCallback(
      { setCount({ it - 1 }) },
      []
    )

    val increment = useCallback(
      { setCount({ it + 1 }) },
      []
    )

    return jsx!{
      <div>
        <button type="button" onClick={decrement}> - </button>
        <span>{count}</span>
        </button> type="button" onClick={increment}> + </button>
      </div>
    }
  }
}


// maybe using a bultin DSL
[FuncComponent]
object Counter implements IFuncComponent {
  static operator func invoke(props: JsObject with CounterProps): JsxElement {
    val [count, setCount] = useState(props.initialCount ?? 0)
  
    val decrement = useCallback(
      { setCount({ it - 1 }) },
      []
    )

    val increment = useCallback(
      { setCount({ it + 1 }) },
      []
    )

    return jsx {
      div {
        button(type="button", onClick=decrement) {
          text " - "
        }
        span {
          text count
        }
        button(type="button", onClick=increment) {
          text " + "
        }
      }
    }
  }
}

// transpiled to javascript ES2015
export class CounterPropsProtocol {
  static get_initialCount(source, defaultValue = null) {
    return source.initialCount ?? defaultValue
  }
}

const Counter = (props) => {
  const [count, setCount] = useState(CounterPropsProtocol.get_initialCount(props) ?? 0);
  
  const decrement = useCallback(
    () => { setCount(it => it - 1) },
    []
  );
  
  const increment = useCallback(
    () => { setCount(it => it + 1) },
    []
  );

  return (
    <div>
      <button type="button" onClick={decrement}> - </button>
      <span>{count}</span>
      </button> type="button" onClick={increment}> + </button>
    </div>
  );
};
```

> TODO Describe the language here

