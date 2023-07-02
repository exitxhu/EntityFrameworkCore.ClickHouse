using EntityFrameworkCore.ClickHouse.FunctionalTests.TestUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.TestUtilities;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace EntityFrameworkCore.ClickHouse.FunctionalTests
{
    public class BuiltInDataTypesClickHouseTest : BuiltInDataTypesTestBase<BuiltInDataTypesClickHouseTest.BuiltInDataTypesClickHouseFixture>
    {
        public BuiltInDataTypesClickHouseTest(BuiltInDataTypesClickHouseFixture fixture) : base(fixture)
        {
        }

        [ConditionalFact]
        public override void Can_insert_and_read_back_all_non_nullable_data_types()
        {
            using (var context = CreateContext())
            {
                context.Set<BuiltInDataTypes>().Add(
                    new BuiltInDataTypes
                    {
                        Id = 1,
                        PartitionId = 1,
                        TestInt16 = -1234,
                        TestInt32 = -123456789,
                        TestInt64 = -1234567890123456789L,
                        TestDouble = -1.23456789,
                        TestDecimal = -1234567890.01M,
                        TestDateTime = DateTime.Parse("01/01/2000 12:34:56"),
                        TestDateTimeOffset = new DateTimeOffset(DateTime.Parse("01/01/2000 12:34:56"), TimeSpan.FromHours(-8.0)),
                        TestTimeSpan = new TimeSpan(0, 10, 9, 8, 7),
                        TestSingle = -1.234F,
                        TestBoolean = true,
                        TestByte = 255,
                        TestUnsignedInt16 = 1234,
                        TestUnsignedInt32 = 1234565789U,
                        TestUnsignedInt64 = 1234567890123456789UL,
                        TestCharacter = 'a',
                        TestSignedByte = -128,
                        Enum64 = Enum64.SomeValue,
                        Enum32 = Enum32.SomeValue,
                        Enum16 = Enum16.SomeValue,
                        Enum8 = Enum8.SomeValue,
                        EnumU64 = EnumU64.SomeValue,
                        EnumU32 = EnumU32.SomeValue,
                        EnumU16 = EnumU16.SomeValue,
                        EnumS8 = EnumS8.SomeValue
                    });

                Assert.Equal(1, context.SaveChanges());
            }

            using (var context = CreateContext())
            {
                var dt = context.Set<BuiltInDataTypes>().Where(e => e.Id == 1).ToList().Single();

                Assert.Equal((short)-1234, dt.TestInt16);
                Assert.Equal(-123456789, dt.TestInt32);
                Assert.Equal(-1234567890123456789L, dt.TestInt64);
                // https://clickhouse.com/docs/en/sql-reference/data-types/float/
                Assert.Equal(-1.23456789, dt.TestDouble, Fixture.DoublePrecision);
                Assert.Equal(-1234567890.01M, dt.TestDecimal, Fixture.DecimalPrecision);
                Assert.Equal(DateTime.Parse("01/01/2000 12:34:56"), dt.TestDateTime);
                Assert.Equal(new DateTimeOffset(DateTime.Parse("01/01/2000 12:34:56"), TimeSpan.FromHours(-8.0)), dt.TestDateTimeOffset);
                Assert.Equal(new TimeSpan(0, 10, 9, 8, 7), dt.TestTimeSpan);
                Assert.Equal(-1.234F, dt.TestSingle);
                Assert.Equal(true, dt.TestBoolean);
                Assert.Equal((byte)255, dt.TestByte);
                Assert.Equal(Enum64.SomeValue, dt.Enum64);
                Assert.Equal(Enum32.SomeValue, dt.Enum32);
                Assert.Equal(Enum16.SomeValue, dt.Enum16);
                Assert.Equal(Enum8.SomeValue, dt.Enum8);
                Assert.Equal((ushort)1234, dt.TestUnsignedInt16);
                Assert.Equal(1234565789U, dt.TestUnsignedInt32);
                Assert.Equal(1234567890123456789UL, dt.TestUnsignedInt64);
                Assert.Equal('a', dt.TestCharacter);
                Assert.Equal((sbyte)-128, dt.TestSignedByte);
                Assert.Equal(EnumU64.SomeValue, dt.EnumU64);
                Assert.Equal(EnumU32.SomeValue, dt.EnumU32);
                Assert.Equal(EnumU16.SomeValue, dt.EnumU16);
                Assert.Equal(EnumS8.SomeValue, dt.EnumS8);
            }
        }
        
        [ConditionalFact]
        public override void Can_insert_and_read_back_all_nullable_data_types_with_values_set_to_non_null()
        {
            using (var context = CreateContext())
            {
                context.Set<BuiltInNullableDataTypes>().Add(
                    new BuiltInNullableDataTypes
                    {
                        Id = 101,
                        PartitionId = 101,
                        TestString = "TestString",
                        TestByteArray = new byte[] { 10, 9, 8, 7, 6 },
                        TestNullableInt16 = -1234,
                        TestNullableInt32 = -123456789,
                        TestNullableInt64 = -1234567890123456789L,
                        TestNullableDouble = -1.23456789,
                        TestNullableDecimal = -1234567890.01M,
                        TestNullableDateTime = DateTime.Parse("01/01/2000 12:34:56"),
                        TestNullableDateTimeOffset =
                            new DateTimeOffset(DateTime.Parse("01/01/2000 12:34:56"), TimeSpan.FromHours(-8.0)),
                        TestNullableTimeSpan = new TimeSpan(0, 10, 9, 8, 7),
                        TestNullableSingle = -1.234F,
                        TestNullableBoolean = false,
                        TestNullableByte = 255,
                        TestNullableUnsignedInt16 = 1234,
                        TestNullableUnsignedInt32 = 1234565789U,
                        TestNullableUnsignedInt64 = 1234567890123456789UL,
                        TestNullableCharacter = 'a',
                        TestNullableSignedByte = -128,
                        Enum64 = Enum64.SomeValue,
                        Enum32 = Enum32.SomeValue,
                        Enum16 = Enum16.SomeValue,
                        Enum8 = Enum8.SomeValue,
                        EnumU64 = EnumU64.SomeValue,
                        EnumU32 = EnumU32.SomeValue,
                        EnumU16 = EnumU16.SomeValue,
                        EnumS8 = EnumS8.SomeValue
                    });

                Assert.Equal(1, context.SaveChanges());
            }

            using (var context = CreateContext())
            {
                var dt = context.Set<BuiltInNullableDataTypes>().Where(ndt => ndt.Id == 101).ToList().Single();

                Assert.Equal("TestString", dt.TestString);
                Assert.Equal(new byte[] { 10, 9, 8, 7, 6 }, dt.TestByteArray);
                Assert.Equal((short)-1234, dt.TestNullableInt16);
                Assert.Equal(-123456789, dt.TestNullableInt32);
                Assert.Equal(-1234567890123456789L, dt.TestNullableInt64);
                // https://clickhouse.com/docs/en/sql-reference/data-types/float/
                Assert.Equal(-1.23456789, dt.TestNullableDouble!.Value, Fixture.DoublePrecision);
                Assert.Equal(-1234567890.01M, dt.TestNullableDecimal!.Value, Fixture.DecimalPrecision);
                Assert.Equal(DateTime.Parse("01/01/2000 12:34:56"), dt.TestNullableDateTime);
                Assert.Equal(new DateTimeOffset(DateTime.Parse("01/01/2000 12:34:56"), TimeSpan.FromHours(-8.0)), dt.TestNullableDateTimeOffset);
                Assert.Equal(new TimeSpan(0, 10, 9, 8, 7), dt.TestNullableTimeSpan);
                Assert.Equal(-1.234F, dt.TestNullableSingle);
                Assert.Equal(false, dt.TestNullableBoolean);
                Assert.Equal((byte)255, dt.TestNullableByte);
                Assert.Equal(Enum64.SomeValue, dt.Enum64);
                Assert.Equal(Enum32.SomeValue, dt.Enum32);
                Assert.Equal(Enum16.SomeValue, dt.Enum16);
                Assert.Equal(Enum8.SomeValue, dt.Enum8);
                Assert.Equal((ushort)1234, dt.TestNullableUnsignedInt16);
                Assert.Equal(1234565789U, dt.TestNullableUnsignedInt32);
                Assert.Equal(1234567890123456789UL, dt.TestNullableUnsignedInt64);
                Assert.Equal('a', dt.TestNullableCharacter);
                Assert.Equal((sbyte)-128, dt.TestNullableSignedByte);
                Assert.Equal(EnumU64.SomeValue, dt.EnumU64);
                Assert.Equal(EnumU32.SomeValue, dt.EnumU32);
                Assert.Equal(EnumU16.SomeValue, dt.EnumU16);
                Assert.Equal(EnumS8.SomeValue, dt.EnumS8);
            }
        }

        [ConditionalFact]
        public override void Can_insert_and_read_back_all_nullable_data_types_with_values_set_to_null()
        {
            using (var context = CreateContext())
            {
                context.Set<BuiltInNullableDataTypes>().Add(
                    new BuiltInNullableDataTypes { Id = 100, PartitionId = 100 });

                Assert.Equal(1, context.SaveChanges());
            }

            using (var context = CreateContext())
            {
                var dt = context.Set<BuiltInNullableDataTypes>().Where(ndt => ndt.Id == 100).ToList().Single();

                Assert.Null(dt.TestString);
                Assert.Empty(dt.TestByteArray); // nullable arrays are not supported
                Assert.Null(dt.TestNullableInt16);
                Assert.Null(dt.TestNullableInt32);
                Assert.Null(dt.TestNullableInt64);
                Assert.Null(dt.TestNullableDouble);
                Assert.Null(dt.TestNullableDecimal);
                Assert.Null(dt.TestNullableDateTime);
                Assert.Null(dt.TestNullableDateTimeOffset);
                Assert.Null(dt.TestNullableTimeSpan);
                Assert.Null(dt.TestNullableSingle);
                Assert.Null(dt.TestNullableBoolean);
                Assert.Null(dt.TestNullableByte);
                Assert.Null(dt.TestNullableUnsignedInt16);
                Assert.Null(dt.TestNullableUnsignedInt32);
                Assert.Null(dt.TestNullableUnsignedInt64);
                Assert.Null(dt.TestNullableCharacter);
                Assert.Null(dt.TestNullableSignedByte);
                Assert.Null(dt.Enum64);
                Assert.Null(dt.Enum32);
                Assert.Null(dt.Enum16);
                Assert.Null(dt.Enum8);
                Assert.Null(dt.EnumU64);
                Assert.Null(dt.EnumU32);
                Assert.Null(dt.EnumU16);
                Assert.Null(dt.EnumS8);
            }
        }
        
        [ConditionalFact]
        public override void Can_insert_and_read_back_non_nullable_backed_data_types()
        {
            using (var context = CreateContext())
            {
                context.Set<NonNullableBackedDataTypes>().Add(
                    new NonNullableBackedDataTypes
                    {
                        Id = 101,
                        PartitionId = 101,
                        Int16 = -1234,
                        Int32 = -123456789,
                        Int64 = -1234567890123456789L,
                        Double = -1.23456789,
                        Decimal = -1234567890.01M,
                        DateTime = DateTime.Parse("01/01/2000 12:34:56"),
                        DateTimeOffset = new DateTimeOffset(DateTime.Parse("01/01/2000 12:34:56"), TimeSpan.FromHours(-8.0)),
                        TimeSpan = new TimeSpan(0, 10, 9, 8, 7),
                        Single = -1.234F,
                        Boolean = true,
                        Byte = 255,
                        UnsignedInt16 = 1234,
                        UnsignedInt32 = 1234565789U,
                        UnsignedInt64 = 1234567890123456789UL,
                        Character = 'a',
                        SignedByte = -128,
                        Enum64 = Enum64.SomeValue,
                        Enum32 = Enum32.SomeValue,
                        Enum16 = Enum16.SomeValue,
                        Enum8 = Enum8.SomeValue,
                        EnumU64 = EnumU64.SomeValue,
                        EnumU32 = EnumU32.SomeValue,
                        EnumU16 = EnumU16.SomeValue,
                        EnumS8 = EnumS8.SomeValue
                    });

                Assert.Equal(1, context.SaveChanges());
            }

            using (var context = CreateContext())
            {
                var dt = context.Set<NonNullableBackedDataTypes>().Where(ndt => ndt.Id == 101).ToList().Single();

                Assert.Equal((short)-1234, dt.Int16);
                Assert.Equal(-123456789, dt.Int32);
                Assert.Equal(-1234567890123456789L, dt.Int64);
                Assert.Equal(-1234567890123456789L, dt.Int64);
                Assert.Equal(-1.23456789, dt.Double!.Value, 8);
                Assert.Equal(-1234567890.01M, dt.Decimal!.Value, Fixture.DecimalPrecision);
                Assert.Equal(DateTime.Parse("01/01/2000 12:34:56"), dt.DateTime);
                Assert.Equal(new DateTimeOffset(DateTime.Parse("01/01/2000 12:34:56"), TimeSpan.FromHours(-8.0)), dt.DateTimeOffset);
                Assert.Equal(new TimeSpan(0, 10, 9, 8, 7), dt.TimeSpan);
                Assert.Equal(-1.234F, dt.Single);
                Assert.Equal(true, dt.Boolean);
                Assert.Equal((byte)255, dt.Byte);
                Assert.Equal(Enum64.SomeValue, dt.Enum64);
                Assert.Equal(Enum32.SomeValue, dt.Enum32);
                Assert.Equal(Enum16.SomeValue, dt.Enum16);
                Assert.Equal(Enum8.SomeValue, dt.Enum8);
                Assert.Equal((ushort)1234, dt.UnsignedInt16);
                Assert.Equal(1234565789U, dt.UnsignedInt32);
                Assert.Equal(1234567890123456789UL, dt.UnsignedInt64);
                Assert.Equal('a', dt.Character);
                Assert.Equal((sbyte)-128, dt.SignedByte);
                Assert.Equal(EnumU64.SomeValue, dt.EnumU64);
                Assert.Equal(EnumU32.SomeValue, dt.EnumU32);
                Assert.Equal(EnumU16.SomeValue, dt.EnumU16);
                Assert.Equal(EnumS8.SomeValue, dt.EnumS8);
            }
        }
        
        [ConditionalFact]
        public override void Can_insert_and_read_back_nullable_backed_data_types()
        {
            using (var context = CreateContext())
            {
                context.Set<NullableBackedDataTypes>().Add(
                    new NullableBackedDataTypes
                    {
                        Id = 101,
                        PartitionId = 101,
                        Int16 = -1234,
                        Int32 = -123456789,
                        Int64 = -1234567890123456789L,
                        Double = -1.23456789,
                        Decimal = -1234567890.01M,
                        DateTime = DateTime.Parse("01/01/2000 12:34:56"),
                        DateTimeOffset = new DateTimeOffset(DateTime.Parse("01/01/2000 12:34:56"), TimeSpan.FromHours(-8.0)),
                        TimeSpan = new TimeSpan(0, 10, 9, 8, 7),
                        Single = -1.234F,
                        Boolean = false,
                        Byte = 255,
                        UnsignedInt16 = 1234,
                        UnsignedInt32 = 1234565789U,
                        UnsignedInt64 = 1234567890123456789UL,
                        Character = 'a',
                        SignedByte = -128,
                        Enum64 = Enum64.SomeValue,
                        Enum32 = Enum32.SomeValue,
                        Enum16 = Enum16.SomeValue,
                        Enum8 = Enum8.SomeValue,
                        EnumU64 = EnumU64.SomeValue,
                        EnumU32 = EnumU32.SomeValue,
                        EnumU16 = EnumU16.SomeValue,
                        EnumS8 = EnumS8.SomeValue
                    });

                Assert.Equal(1, context.SaveChanges());
            }

            using (var context = CreateContext())
            {
                var dt = context.Set<NullableBackedDataTypes>().Where(ndt => ndt.Id == 101).ToList().Single();

                Assert.Equal((short)-1234, dt.Int16);
                Assert.Equal(-123456789, dt.Int32);
                Assert.Equal(-1234567890123456789L, dt.Int64);
                Assert.Equal(-1.23456789, dt.Double, 8);
                Assert.Equal(-1234567890.01M, dt.Decimal, Fixture.DecimalPrecision);
                Assert.Equal(DateTime.Parse("01/01/2000 12:34:56"), dt.DateTime);
                Assert.Equal(new DateTimeOffset(DateTime.Parse("01/01/2000 12:34:56"), TimeSpan.FromHours(-8.0)), dt.DateTimeOffset);
                Assert.Equal(new TimeSpan(0, 10, 9, 8, 7), dt.TimeSpan);
                Assert.Equal(-1.234F, dt.Single);
                Assert.Equal(false, dt.Boolean);
                Assert.Equal((byte)255, dt.Byte);
                Assert.Equal(Enum64.SomeValue, dt.Enum64);
                Assert.Equal(Enum32.SomeValue, dt.Enum32);
                Assert.Equal(Enum16.SomeValue, dt.Enum16);
                Assert.Equal(Enum8.SomeValue, dt.Enum8);
                Assert.Equal((ushort)1234, dt.UnsignedInt16);
                Assert.Equal(1234565789U, dt.UnsignedInt32);
                Assert.Equal(1234567890123456789UL, dt.UnsignedInt64);
                Assert.Equal('a', dt.Character);
                Assert.Equal((sbyte)-128, dt.SignedByte);
                Assert.Equal(EnumU64.SomeValue, dt.EnumU64);
                Assert.Equal(EnumU32.SomeValue, dt.EnumU32);
                Assert.Equal(EnumU16.SomeValue, dt.EnumU16);
                Assert.Equal(EnumS8.SomeValue, dt.EnumS8);
            }
        }
        
        [ConditionalFact]
        public override void Can_insert_and_read_back_object_backed_data_types()
        {
            using (var context = CreateContext())
            {
                context.Set<ObjectBackedDataTypes>().Add(
                    new ObjectBackedDataTypes
                    {
                        Id = 101,
                        PartitionId = 101,
                        String = "TestString",
                        Bytes = new byte[] { 10, 9, 8, 7, 6 },
                        Int16 = -1234,
                        Int32 = -123456789,
                        Int64 = -1234567890123456789L,
                        Double = -1.23456789,
                        Decimal = -1234567890.01M,
                        DateTime = DateTime.Parse("01/01/2000 12:34:56"),
                        DateTimeOffset = new DateTimeOffset(DateTime.Parse("01/01/2000 12:34:56"), TimeSpan.FromHours(-8.0)),
                        TimeSpan = new TimeSpan(0, 10, 9, 8, 7),
                        Single = -1.234F,
                        Boolean = false,
                        Byte = 255,
                        UnsignedInt16 = 1234,
                        UnsignedInt32 = 1234565789U,
                        UnsignedInt64 = 1234567890123456789UL,
                        Character = 'a',
                        SignedByte = -128,
                        Enum64 = Enum64.SomeValue,
                        Enum32 = Enum32.SomeValue,
                        Enum16 = Enum16.SomeValue,
                        Enum8 = Enum8.SomeValue,
                        EnumU64 = EnumU64.SomeValue,
                        EnumU32 = EnumU32.SomeValue,
                        EnumU16 = EnumU16.SomeValue,
                        EnumS8 = EnumS8.SomeValue
                    });

                Assert.Equal(1, context.SaveChanges());
            }

            using (var context = CreateContext())
            {
                var dt = context.Set<ObjectBackedDataTypes>().Where(ndt => ndt.Id == 101).ToList().Single();

                Assert.Equal("TestString", dt.String);
                Assert.Equal(new byte[] { 10, 9, 8, 7, 6 }, dt.Bytes);
                Assert.Equal((short)-1234, dt.Int16);
                Assert.Equal(-123456789, dt.Int32);
                Assert.Equal(-1234567890123456789L, dt.Int64);
                Assert.Equal(-1.23456789, dt.Double, Fixture.DoublePrecision);
                Assert.Equal(-1234567890.01M, dt.Decimal, Fixture.DecimalPrecision);
                Assert.Equal(DateTime.Parse("01/01/2000 12:34:56"), dt.DateTime);
                Assert.Equal(new DateTimeOffset(DateTime.Parse("01/01/2000 12:34:56"), TimeSpan.FromHours(-8.0)), dt.DateTimeOffset);
                Assert.Equal(new TimeSpan(0, 10, 9, 8, 7), dt.TimeSpan);
                Assert.Equal(-1.234F, dt.Single);
                Assert.Equal(false, dt.Boolean);
                Assert.Equal((byte)255, dt.Byte);
                Assert.Equal(Enum64.SomeValue, dt.Enum64);
                Assert.Equal(Enum32.SomeValue, dt.Enum32);
                Assert.Equal(Enum16.SomeValue, dt.Enum16);
                Assert.Equal(Enum8.SomeValue, dt.Enum8);
                Assert.Equal((ushort)1234, dt.UnsignedInt16);
                Assert.Equal(1234565789U, dt.UnsignedInt32);
                Assert.Equal(1234567890123456789UL, dt.UnsignedInt64);
                Assert.Equal('a', dt.Character);
                Assert.Equal((sbyte)-128, dt.SignedByte);
                Assert.Equal(EnumU64.SomeValue, dt.EnumU64);
                Assert.Equal(EnumU32.SomeValue, dt.EnumU32);
                Assert.Equal(EnumU16.SomeValue, dt.EnumU16);
                Assert.Equal(EnumS8.SomeValue, dt.EnumS8);
            }
        }
        
        [ConditionalFact]
        public override void Can_insert_and_read_back_with_null_binary_foreign_key()
        {
            using (var context = CreateContext())
            {
                context.Set<BinaryForeignKeyDataType>().Add(
                    new BinaryForeignKeyDataType { Id = 78 });

                Assert.Equal(1, context.SaveChanges());
            }

            using (var context = CreateContext())
            {
                var entity = context.Set<BinaryForeignKeyDataType>().Where(e => e.Id == 78).ToList().Single();
                // nullable arrays are not supported
                Assert.Empty(entity.BinaryKeyDataTypeId);
            }
        }
        
        [ConditionalFact]
        public override void Can_query_using_any_data_type()
        {
            using var context = CreateContext();
            var source = AddTestBuiltInDataTypes(context.Set<BuiltInDataTypes>());

            Assert.Equal(1, context.SaveChanges());

            QueryBuiltInDataTypesTest(source);
        }
        
        [ConditionalFact]
        public override void Can_query_using_any_data_type_nullable_shadow()
        {
            using var context = CreateContext();
            var source = AddTestBuiltInNullableDataTypes(context.Set<BuiltInNullableDataTypesShadow>());

            Assert.Equal(1, context.SaveChanges());

            QueryBuiltInNullableDataTypesTest(source);
        }
        
        [ConditionalFact]
        public override void Can_query_using_any_nullable_data_type()
        {
            using var context = CreateContext();
            var source = AddTestBuiltInNullableDataTypes(context.Set<BuiltInNullableDataTypes>());

            Assert.Equal(1, context.SaveChanges());

            QueryBuiltInNullableDataTypesTest(source);
        }
        
        [ConditionalFact]
        public override void Can_query_using_any_data_type_shadow()
        {
            using var context = CreateContext();
            var source = AddTestBuiltInDataTypes(context.Set<BuiltInDataTypesShadow>());

            Assert.Equal(1, context.SaveChanges());

            QueryBuiltInDataTypesTest(source);
        }

        [ConditionalFact]
        public override void Can_query_using_any_nullable_data_type_as_literal()
        {
            using (var context = CreateContext())
            {
                context.Set<BuiltInNullableDataTypes>().Add(
                    new BuiltInNullableDataTypes
                    {
                        Id = 12,
                        PartitionId = 1,
                        TestNullableInt16 = -1234,
                        TestNullableInt32 = -123456789,
                        TestNullableInt64 = -1234567890123456789L,
                        TestNullableDouble = -1.23456789,
                        TestNullableDecimal = -1234567890.01M,
                        TestNullableDateTime = Fixture.DefaultDateTime,
                        TestNullableDateTimeOffset = new DateTimeOffset(new DateTime(), TimeSpan.FromHours(-8.0)),
                        TestNullableTimeSpan = new TimeSpan(0, 10, 9, 8, 7),
                        TestNullableSingle = -1.234F,
                        TestNullableBoolean = true,
                        TestNullableByte = 255,
                        TestNullableUnsignedInt16 = 1234,
                        TestNullableUnsignedInt32 = 1234565789U,
                        TestNullableUnsignedInt64 = 1234567890123456789UL,
                        TestNullableCharacter = 'a',
                        TestNullableSignedByte = -128,
                        Enum64 = Enum64.SomeValue,
                        Enum32 = Enum32.SomeValue,
                        Enum16 = Enum16.SomeValue,
                        Enum8 = Enum8.SomeValue,
                        EnumU64 = EnumU64.SomeValue,
                        EnumU32 = EnumU32.SomeValue,
                        EnumU16 = EnumU16.SomeValue,
                        EnumS8 = EnumS8.SomeValue
                    });

                Assert.Equal(1, context.SaveChanges());
            }

            using (var context = CreateContext())
            {
                var entity = context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12).ToList().Single();
                var entityType = context.Model.FindEntityType(typeof(BuiltInNullableDataTypes));

                Assert.Same(
                    entity,
                    context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && e.TestNullableInt16 == -1234).ToList().Single());

                Assert.Same(
                    entity,
                    context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && e.TestNullableInt32 == -123456789).ToList().Single());

                Assert.Same(
                    entity,
                    context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && e.TestNullableInt64 == -1234567890123456789L).ToList()
                        .Single());

                if (Fixture.StrictEquality)
                {
                    Assert.Same(
                        entity,
                        context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && e.TestNullableDouble == -1.23456789).ToList()
                            .Single());
                }
                else if (Fixture.SupportsDecimalComparisons)
                {
                    Assert.Same(
                        entity,
                        context.Set<BuiltInNullableDataTypes>().Where(
                            e => e.Id == 12
                                && -e.TestNullableDouble + -1.23456789 < 1E-5
                                && -e.TestNullableDouble + -1.23456789 > -1E-5).ToList().Single());
                }

                Assert.Same(
                    entity,
                    context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && e.TestNullableDouble != 1E18).ToList().Single());

                /* TODO Needs research
                Assert.Same(
                    entity,
                    context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && e.TestNullableDecimal == -1234567890.01M).ToList()
                        .Single());
                */

                Assert.Same(
                    entity,
                    context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && e.TestNullableDateTime == Fixture.DefaultDateTime)
                        .ToList().Single());

                if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.TestNullableDateTimeOffset)) != null)
                {
                    Assert.Same(
                        entity,
                        context.Set<BuiltInNullableDataTypes>().Where(
                                e => e.Id == 12
                                    && e.TestNullableDateTimeOffset
                                    == new DateTimeOffset(new DateTime(), TimeSpan.FromHours(-8.0)))
                            .ToList().Single());
                }

                if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.TestNullableTimeSpan)) != null)
                {
                    Assert.Same(
                        entity,
                        context.Set<BuiltInNullableDataTypes>()
                            .Where(e => e.Id == 12 && e.TestNullableTimeSpan == new TimeSpan(0, 10, 9, 8, 7)).ToList().Single());
                }

                if (Fixture.StrictEquality)
                {
                    Assert.Same(
                        entity,
                        context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && e.TestNullableSingle == -1.234F).ToList()
                            .Single());
                }
                else if (Fixture.SupportsDecimalComparisons)
                {
                    Assert.Same(
                        entity,
                        context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && -e.TestNullableSingle + -1.234F < 1E-5).ToList()
                            .Single());
                }

                Assert.Same(
                    entity,
                    context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && e.TestNullableSingle != 1E-8).ToList().Single());

                Assert.Same(
                    entity,
                    context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && e.TestNullableBoolean == true).ToList().Single());

                if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.TestNullableByte)) != null)
                {
                    Assert.Same(
                        entity,
                        context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && e.TestNullableByte == 255).ToList().Single());
                }

                Assert.Same(
                    entity,
                    context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && e.Enum64 == Enum64.SomeValue).ToList().Single());

                Assert.Same(
                    entity,
                    context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && e.Enum32 == Enum32.SomeValue).ToList().Single());

                Assert.Same(
                    entity,
                    context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && e.Enum16 == Enum16.SomeValue).ToList().Single());

                if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.Enum8)) != null)
                {
                    Assert.Same(
                        entity,
                        context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && e.Enum8 == Enum8.SomeValue).ToList().Single());
                }

                if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.TestNullableUnsignedInt16)) != null)
                {
                    Assert.Same(
                        entity,
                        context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && e.TestNullableUnsignedInt16 == 1234).ToList()
                            .Single());
                }

                if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.TestNullableUnsignedInt32)) != null)
                {
                    Assert.Same(
                        entity,
                        context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && e.TestNullableUnsignedInt32 == 1234565789U)
                            .ToList().Single());
                }

                if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.TestNullableUnsignedInt64)) != null)
                {
                    Assert.Same(
                        entity,
                        context.Set<BuiltInNullableDataTypes>()
                            .Where(e => e.Id == 12 && e.TestNullableUnsignedInt64 == 1234567890123456789UL).ToList().Single());
                }

                if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.TestNullableCharacter)) != null)
                {
                    Assert.Same(
                        entity,
                        context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && e.TestNullableCharacter == 'a').ToList().Single());
                }

                if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.TestNullableSignedByte)) != null)
                {
                    Assert.Same(
                        entity,
                        context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && e.TestNullableSignedByte == -128).ToList()
                            .Single());
                }

                if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.EnumU64)) != null)
                {
                    Assert.Same(
                        entity,
                        context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && e.EnumU64 == EnumU64.SomeValue).ToList().Single());
                }

                if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.EnumU32)) != null)
                {
                    Assert.Same(
                        entity,
                        context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && e.EnumU32 == EnumU32.SomeValue).ToList().Single());
                }

                if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.EnumU16)) != null)
                {
                    Assert.Same(
                        entity,
                        context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && e.EnumU16 == EnumU16.SomeValue).ToList().Single());
                }

                if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.EnumS8)) != null)
                {
                    Assert.Same(
                        entity,
                        context.Set<BuiltInNullableDataTypes>().Where(e => e.Id == 12 && e.EnumS8 == EnumS8.SomeValue).ToList().Single());
                }
            }
        }

        [ConditionalFact(Skip = "Fails often")]
        public override void Optional_datetime_reading_null_from_database()
        {
            base.Optional_datetime_reading_null_from_database();
            
            using var context = CreateContext();
            var expected = context.Set<DateTimeEnclosure>().ToList()
                .Select(e => new { DT = e.DateTimeOffset == null ? (DateTime?)null : e.DateTimeOffset.Value.DateTime.Date }).ToList();

            var actual = context.Set<DateTimeEnclosure>()
                .Select(e => new { DT = e.DateTimeOffset == null ? (DateTime?)null : e.DateTimeOffset.Value.DateTime.Date }).ToList();

            for (var i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].DT, actual[i].DT);
            }
        }

        [ConditionalFact(Skip = "Foreign keys are not supported")]
        public override void Can_read_back_mapped_enum_from_collection_first_or_default()
        {
        }
        
        private void QueryBuiltInDataTypesTest<TEntity>(EntityEntry<TEntity> source)
            where TEntity : BuiltInDataTypesBase
        {
            using var context = CreateContext();
            var set = context.Set<TEntity>();
            var entity = set.Where(e => e.Id == 11).ToList().Single();
            var entityType = context.Model.FindEntityType(typeof(TEntity));

            var param1 = (short)-1234;
            Assert.Same(
                entity,
                set.Where(e => e.Id == 11 && EF.Property<short>(e, nameof(BuiltInDataTypes.TestInt16)) == param1).ToList().Single());

            var param2 = -123456789;
            Assert.Same(
                entity,
                set.Where(e => e.Id == 11 && EF.Property<int>(e, nameof(BuiltInDataTypes.TestInt32)) == param2).ToList().Single());

            var param3 = -1234567890123456789L;
            if (Fixture.IntegerPrecision == 64)
            {
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<long>(e, nameof(BuiltInDataTypes.TestInt64)) == param3).ToList().Single());
            }

            double? param4 = -1.23456789;
            if (Fixture.StrictEquality)
            {
                Assert.Same(
                    entity, set.Where(
                        e => e.Id == 11
                            && EF.Property<double>(e, nameof(BuiltInDataTypes.TestDouble)) == param4).ToList().Single());
            }
            else if (Fixture.SupportsDecimalComparisons)
            {
                double? param4l = -1.234567891;
                double? param4h = -1.234567889;
                Assert.Same(
                    entity, set.Where(
                            e => e.Id == 11
                                && (EF.Property<double>(e, nameof(BuiltInDataTypes.TestDouble)) == param4
                                    || (EF.Property<double>(e, nameof(BuiltInDataTypes.TestDouble)) > param4l
                                        && EF.Property<double>(e, nameof(BuiltInDataTypes.TestDouble)) < param4h)))
                        .ToList().Single());
            }

            var param5 = -1234567890.01M;
            Assert.Same(
                entity,
                set.Where(e => e.Id == 11 && EF.Property<decimal>(e, nameof(BuiltInDataTypes.TestDecimal)) == param5).ToList()
                    .Single());

            var param6 = Fixture.DefaultDateTime;
            Assert.Same(
                entity,
                set.Where(e => e.Id == 11 && EF.Property<DateTime>(e, nameof(BuiltInDataTypes.TestDateTime)) == param6).ToList()
                    .Single());

            if (entityType.FindProperty(nameof(BuiltInDataTypes.TestDateTimeOffset)) != null)
            {
                var param7 = new DateTimeOffset(new DateTime(), TimeSpan.FromHours(-8.0));
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<DateTimeOffset>(e, nameof(BuiltInDataTypes.TestDateTimeOffset)) == param7)
                        .ToList().Single());
            }

            if (entityType.FindProperty(nameof(BuiltInDataTypes.TestTimeSpan)) != null)
            {
                var param8 = new TimeSpan(0, 10, 9, 8, 7);
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<TimeSpan>(e, nameof(BuiltInDataTypes.TestTimeSpan)) == param8).ToList()
                        .Single());
            }

            var param9 = -1.234F;
            if (Fixture.StrictEquality)
            {
                Assert.Same(
                    entity, set.Where(
                        e => e.Id == 11
                            && EF.Property<float>(e, nameof(BuiltInDataTypes.TestSingle)) == param9).ToList().Single());
            }
            else if (Fixture.SupportsDecimalComparisons)
            {
                var param9l = -1.2341F;
                var param9h = -1.2339F;
                Assert.Same(
                    entity, set.Where(
                        e => e.Id == 11
                            && (EF.Property<float>(e, nameof(BuiltInDataTypes.TestSingle)) == param9
                                || (EF.Property<float>(e, nameof(BuiltInDataTypes.TestSingle)) > param9l
                                    && EF.Property<float>(e, nameof(BuiltInDataTypes.TestSingle)) < param9h))).ToList().Single());
            }

            var param10 = true;
            Assert.Same(
                entity,
                set.Where(e => e.Id == 11 && EF.Property<bool>(e, nameof(BuiltInDataTypes.TestBoolean)) == param10).ToList().Single());

            if (entityType.FindProperty(nameof(BuiltInDataTypes.TestByte)) != null)
            {
                var param11 = (byte)255;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<byte>(e, nameof(BuiltInDataTypes.TestByte)) == param11).ToList().Single());
            }

            var param12 = Enum64.SomeValue;
            Assert.Same(
                entity,
                set.Where(e => e.Id == 11 && EF.Property<Enum64>(e, nameof(BuiltInDataTypes.Enum64)) == param12).ToList().Single());

            var param13 = Enum32.SomeValue;
            Assert.Same(
                entity,
                set.Where(e => e.Id == 11 && EF.Property<Enum32>(e, nameof(BuiltInDataTypes.Enum32)) == param13).ToList().Single());

            var param14 = Enum16.SomeValue;
            Assert.Same(
                entity,
                set.Where(e => e.Id == 11 && EF.Property<Enum16>(e, nameof(BuiltInDataTypes.Enum16)) == param14).ToList().Single());

            if (entityType.FindProperty(nameof(BuiltInDataTypes.Enum8)) != null)
            {
                var param15 = Enum8.SomeValue;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<Enum8>(e, nameof(BuiltInDataTypes.Enum8)) == param15).ToList().Single());
            }

            if (entityType.FindProperty(nameof(BuiltInDataTypes.TestUnsignedInt16)) != null)
            {
                var param16 = (ushort)1234;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<ushort>(e, nameof(BuiltInDataTypes.TestUnsignedInt16)) == param16).ToList()
                        .Single());
            }

            if (entityType.FindProperty(nameof(BuiltInDataTypes.TestUnsignedInt32)) != null)
            {
                var param17 = 1234565789U;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<uint>(e, nameof(BuiltInDataTypes.TestUnsignedInt32)) == param17).ToList()
                        .Single());
            }

            if (entityType.FindProperty(nameof(BuiltInDataTypes.TestUnsignedInt64)) != null)
            {
                var param18 = 1234567890123456789UL;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<ulong>(e, nameof(BuiltInDataTypes.TestUnsignedInt64)) == param18).ToList()
                        .Single());
            }

            if (entityType.FindProperty(nameof(BuiltInDataTypes.TestCharacter)) != null)
            {
                var param19 = 'a';
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<char>(e, nameof(BuiltInDataTypes.TestCharacter)) == param19).ToList()
                        .Single());
            }

            if (entityType.FindProperty(nameof(BuiltInDataTypes.TestSignedByte)) != null)
            {
                var param20 = (sbyte)-128;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<sbyte>(e, nameof(BuiltInDataTypes.TestSignedByte)) == param20).ToList()
                        .Single());
            }

            if (entityType.FindProperty(nameof(BuiltInDataTypes.EnumU64)) != null)
            {
                var param21 = EnumU64.SomeValue;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<EnumU64>(e, nameof(BuiltInDataTypes.EnumU64)) == param21).ToList()
                        .Single());
            }

            if (entityType.FindProperty(nameof(BuiltInDataTypes.EnumU32)) != null)
            {
                var param22 = EnumU32.SomeValue;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<EnumU32>(e, nameof(BuiltInDataTypes.EnumU32)) == param22).ToList()
                        .Single());
            }

            if (entityType.FindProperty(nameof(BuiltInDataTypes.EnumU16)) != null)
            {
                var param23 = EnumU16.SomeValue;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<EnumU16>(e, nameof(BuiltInDataTypes.EnumU16)) == param23).ToList()
                        .Single());
            }

            if (entityType.FindProperty(nameof(BuiltInDataTypes.EnumS8)) != null)
            {
                var param24 = EnumS8.SomeValue;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<EnumS8>(e, nameof(BuiltInDataTypes.EnumS8)) == param24).ToList().Single());
            }

            if (UnwrapNullableType(entityType.FindProperty(nameof(BuiltInDataTypes.Enum64))?.GetProviderClrType()) == typeof(long))
            {
                var param25 = 1;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<Enum64>(e, nameof(BuiltInDataTypes.Enum64)) == (Enum64)param25).ToList()
                        .Single());
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && (int)EF.Property<Enum64>(e, nameof(BuiltInDataTypes.Enum64)) == param25).ToList()
                        .Single());
            }

            if (UnwrapNullableType(entityType.FindProperty(nameof(BuiltInDataTypes.Enum32))?.GetProviderClrType()) == typeof(int))
            {
                var param26 = 1;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<Enum32>(e, nameof(BuiltInDataTypes.Enum32)) == (Enum32)param26).ToList()
                        .Single());
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && (int)EF.Property<Enum32>(e, nameof(BuiltInDataTypes.Enum32)) == param26).ToList()
                        .Single());
            }

            if (UnwrapNullableType(entityType.FindProperty(nameof(BuiltInDataTypes.Enum16))?.GetProviderClrType()) == typeof(short))
            {
                var param27 = 1;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<Enum16>(e, nameof(BuiltInDataTypes.Enum16)) == (Enum16)param27).ToList()
                        .Single());
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && (int)EF.Property<Enum16>(e, nameof(BuiltInDataTypes.Enum16)) == param27).ToList()
                        .Single());
            }

            if (UnwrapNullableType(entityType.FindProperty(nameof(BuiltInDataTypes.Enum8))?.GetProviderClrType()) == typeof(byte))
            {
                var param28 = 1;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<Enum8>(e, nameof(BuiltInDataTypes.Enum8)) == (Enum8)param28).ToList()
                        .Single());
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && (int)EF.Property<Enum8>(e, nameof(BuiltInDataTypes.Enum8)) == param28).ToList()
                        .Single());
            }

            AssertProperties(source, context, entity);
        }
        
        private void QueryBuiltInNullableDataTypesTest<TEntity>(EntityEntry<TEntity> source)
            where TEntity : BuiltInNullableDataTypesBase
        {
            using var context = CreateContext();
            var set = context.Set<TEntity>();
            var entity = set.Where(e => e.Id == 11).ToList().Single();
            var entityType = context.Model.FindEntityType(typeof(TEntity));

            short? param1 = -1234;
            Assert.Same(
                entity,
                set.Where(e => e.Id == 11 && EF.Property<short?>(e, nameof(BuiltInNullableDataTypes.TestNullableInt16)) == param1)
                    .ToList().Single());

            int? param2 = -123456789;
            Assert.Same(
                entity,
                set.Where(e => e.Id == 11 && EF.Property<int?>(e, nameof(BuiltInNullableDataTypes.TestNullableInt32)) == param2)
                    .ToList().Single());

            long? param3 = -1234567890123456789L;
            Assert.Same(
                entity,
                set.Where(e => e.Id == 11 && EF.Property<long?>(e, nameof(BuiltInNullableDataTypes.TestNullableInt64)) == param3)
                    .ToList().Single());

            double? param4 = -1.23456789;
            if (Fixture.StrictEquality)
            {
                Assert.Same(
                    entity, set.Where(
                            e => e.Id == 11
                                && EF.Property<double?>(e, nameof(BuiltInNullableDataTypes.TestNullableDouble)) == param4).ToList()
                        .Single());
            }
            else if (Fixture.SupportsDecimalComparisons)
            {
                double? param4l = -1.234567891;
                double? param4h = -1.234567889;
                Assert.Same(
                    entity, set.Where(
                            e => e.Id == 11
                                && (EF.Property<double?>(e, nameof(BuiltInNullableDataTypes.TestNullableDouble)) == param4
                                    || (EF.Property<double?>(e, nameof(BuiltInNullableDataTypes.TestNullableDouble)) > param4l
                                        && EF.Property<double?>(e, nameof(BuiltInNullableDataTypes.TestNullableDouble)) < param4h)))
                        .ToList().Single());
            }

            decimal? param5 = -1234567890.01M;
            Assert.Same(
                entity,
                set.Where(e => e.Id == 11 && EF.Property<decimal?>(e, nameof(BuiltInNullableDataTypes.TestNullableDecimal)) == param5)
                    .ToList().Single());

            DateTime? param6 = Fixture.DefaultDateTime;
            Assert.Same(
                entity,
                set.Where(e => e.Id == 11 && EF.Property<DateTime?>(e, nameof(BuiltInNullableDataTypes.TestNullableDateTime)) == param6)
                    .ToList().Single());

            if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.TestNullableDateTimeOffset)) != null)
            {
                DateTimeOffset? param7 = new DateTimeOffset(new DateTime(), TimeSpan.FromHours(-8.0));
                Assert.Same(
                    entity,
                    set.Where(
                        e => e.Id == 11
                            && EF.Property<DateTimeOffset?>(e, nameof(BuiltInNullableDataTypes.TestNullableDateTimeOffset))
                            == param7).ToList().Single());
            }

            if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.TestNullableTimeSpan)) != null)
            {
                TimeSpan? param8 = new TimeSpan(0, 10, 9, 8, 7);
                Assert.Same(
                    entity,
                    set.Where(
                            e => e.Id == 11
                                && EF.Property<TimeSpan?>(e, nameof(BuiltInNullableDataTypes.TestNullableTimeSpan))
                                == param8)
                        .ToList().Single());
            }

            float? param9 = -1.234F;
            if (Fixture.StrictEquality)
            {
                Assert.Same(
                    entity, set.Where(
                            e => e.Id == 11
                                && EF.Property<float?>(e, nameof(BuiltInNullableDataTypes.TestNullableSingle)) == param9).ToList()
                        .Single());
            }
            else if (Fixture.SupportsDecimalComparisons)
            {
                float? param9l = -1.2341F;
                float? param9h = -1.2339F;
                Assert.Same(
                    entity, set.Where(
                            e => e.Id == 11
                                && (EF.Property<float?>(e, nameof(BuiltInNullableDataTypes.TestNullableSingle)) == param9
                                    || (EF.Property<float?>(e, nameof(BuiltInNullableDataTypes.TestNullableSingle)) > param9l
                                        && EF.Property<float?>(e, nameof(BuiltInNullableDataTypes.TestNullableSingle)) < param9h)))
                        .ToList().Single());
            }

            bool? param10 = true;
            Assert.Same(
                entity,
                set.Where(e => e.Id == 11 && EF.Property<bool?>(e, nameof(BuiltInNullableDataTypes.TestNullableBoolean)) == param10)
                    .ToList().Single());

            if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.TestNullableByte)) != null)
            {
                byte? param11 = 255;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<byte?>(e, nameof(BuiltInNullableDataTypes.TestNullableByte)) == param11)
                        .ToList().Single());
            }

            Enum64? param12 = Enum64.SomeValue;
            Assert.Same(
                entity,
                set.Where(e => e.Id == 11 && EF.Property<Enum64?>(e, nameof(BuiltInNullableDataTypes.Enum64)) == param12).ToList()
                    .Single());

            Enum32? param13 = Enum32.SomeValue;
            Assert.Same(
                entity,
                set.Where(e => e.Id == 11 && EF.Property<Enum32?>(e, nameof(BuiltInNullableDataTypes.Enum32)) == param13).ToList()
                    .Single());

            Enum16? param14 = Enum16.SomeValue;
            Assert.Same(
                entity,
                set.Where(e => e.Id == 11 && EF.Property<Enum16?>(e, nameof(BuiltInNullableDataTypes.Enum16)) == param14).ToList()
                    .Single());

            if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.Enum8)) != null)
            {
                Enum8? param15 = Enum8.SomeValue;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<Enum8?>(e, nameof(BuiltInNullableDataTypes.Enum8)) == param15).ToList()
                        .Single());
            }

            if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.TestNullableUnsignedInt16)) != null)
            {
                ushort? param16 = 1234;
                Assert.Same(
                    entity,
                    set.Where(
                        e => e.Id == 11
                            && EF.Property<ushort?>(e, nameof(BuiltInNullableDataTypes.TestNullableUnsignedInt16))
                            == param16).ToList().Single());
            }

            if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.TestNullableUnsignedInt32)) != null)
            {
                uint? param17 = 1234565789U;
                Assert.Same(
                    entity,
                    set.Where(
                            e => e.Id == 11
                                && EF.Property<uint?>(e, nameof(BuiltInNullableDataTypes.TestNullableUnsignedInt32))
                                == param17)
                        .ToList().Single());
            }

            if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.TestNullableUnsignedInt64)) != null)
            {
                ulong? param18 = 1234567890123456789UL;
                Assert.Same(
                    entity,
                    set.Where(
                        e => e.Id == 11
                            && EF.Property<ulong?>(
                                e, nameof(BuiltInNullableDataTypes.TestNullableUnsignedInt64))
                            == param18).ToList().Single());
            }

            if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.TestNullableCharacter)) != null)
            {
                char? param19 = 'a';
                Assert.Same(
                    entity,
                    set.Where(
                            e => e.Id == 11 && EF.Property<char?>(e, nameof(BuiltInNullableDataTypes.TestNullableCharacter)) == param19)
                        .ToList().Single());
            }

            if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.TestNullableSignedByte)) != null)
            {
                sbyte? param20 = -128;
                Assert.Same(
                    entity,
                    set.Where(
                            e => e.Id == 11
                                && EF.Property<sbyte?>(e, nameof(BuiltInNullableDataTypes.TestNullableSignedByte))
                                == param20)
                        .ToList().Single());
            }

            if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.EnumU64)) != null)
            {
                var param21 = EnumU64.SomeValue;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<EnumU64?>(e, nameof(BuiltInNullableDataTypes.EnumU64)) == param21).ToList()
                        .Single());
            }

            if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.EnumU32)) != null)
            {
                var param22 = EnumU32.SomeValue;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<EnumU32?>(e, nameof(BuiltInNullableDataTypes.EnumU32)) == param22).ToList()
                        .Single());
            }

            if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.EnumU16)) != null)
            {
                var param23 = EnumU16.SomeValue;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<EnumU16?>(e, nameof(BuiltInNullableDataTypes.EnumU16)) == param23).ToList()
                        .Single());
            }

            if (entityType.FindProperty(nameof(BuiltInNullableDataTypes.EnumS8)) != null)
            {
                var param24 = EnumS8.SomeValue;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<EnumS8?>(e, nameof(BuiltInNullableDataTypes.EnumS8)) == param24).ToList()
                        .Single());
            }

            if (UnwrapNullableType(entityType.FindProperty(nameof(BuiltInNullableDataTypes.Enum64))?.GetProviderClrType())
                == typeof(long))
            {
                int? param25 = 1;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<Enum64?>(e, nameof(BuiltInNullableDataTypes.Enum64)) == (Enum64)param25)
                        .ToList().Single());
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && (int)EF.Property<Enum64?>(e, nameof(BuiltInNullableDataTypes.Enum64)) == param25)
                        .ToList().Single());
            }

            if (UnwrapNullableType(entityType.FindProperty(nameof(BuiltInNullableDataTypes.Enum32))?.GetProviderClrType())
                == typeof(int))
            {
                int? param26 = 1;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<Enum32?>(e, nameof(BuiltInNullableDataTypes.Enum32)) == (Enum32)param26)
                        .ToList().Single());
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && (int)EF.Property<Enum32?>(e, nameof(BuiltInNullableDataTypes.Enum32)) == param26)
                        .ToList().Single());
            }

            if (UnwrapNullableType(entityType.FindProperty(nameof(BuiltInNullableDataTypes.Enum16))?.GetProviderClrType())
                == typeof(short))
            {
                int? param27 = 1;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<Enum16?>(e, nameof(BuiltInNullableDataTypes.Enum16)) == (Enum16)param27)
                        .ToList().Single());
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && (int)EF.Property<Enum16?>(e, nameof(BuiltInNullableDataTypes.Enum16)) == param27)
                        .ToList().Single());
            }

            if (UnwrapNullableType(entityType.FindProperty(nameof(BuiltInNullableDataTypes.Enum8))?.GetProviderClrType())
                == typeof(byte))
            {
                int? param28 = 1;
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && EF.Property<Enum8?>(e, nameof(BuiltInNullableDataTypes.Enum8)) == (Enum8)param28)
                        .ToList().Single());
                Assert.Same(
                    entity,
                    set.Where(e => e.Id == 11 && (int)EF.Property<Enum8?>(e, nameof(BuiltInNullableDataTypes.Enum8)) == param28)
                        .ToList().Single());
            }

            AssertProperties(source, context, entity);
        }

        private void AssertProperties<TEntity>(EntityEntry<TEntity> source, DbContext context, TEntity entity) where TEntity: class
        {
            foreach (var propertyEntry in context.Entry(entity).Properties)
            {
                if (propertyEntry.Metadata.ValueGenerated != ValueGenerated.Never)
                {
                    continue;
                }

                if (propertyEntry.CurrentValue is double)
                {
                    Assert.Equal(
                        (double)source.Property(propertyEntry.Metadata.Name).CurrentValue,
                        (double)propertyEntry.CurrentValue,
                        Fixture.DoublePrecision);
                }
                else if (propertyEntry.CurrentValue is decimal)
                {
                    Assert.Equal(
                        (decimal)source.Property(propertyEntry.Metadata.Name).CurrentValue,
                        (decimal)propertyEntry.CurrentValue,
                        Fixture.DecimalPrecision);
                }
                else if (propertyEntry.CurrentValue is Array && source.Property(propertyEntry.Metadata.Name).CurrentValue is null)
                {
                    Assert.Empty((IEnumerable)propertyEntry.CurrentValue);
                }
                else
                {
                    Assert.Equal(
                        source.Property(propertyEntry.Metadata.Name).CurrentValue,
                        propertyEntry.CurrentValue);
                }
            }
        }

        private static Type UnwrapNullableType(Type type)
            => type == null ? null : Nullable.GetUnderlyingType(type) ?? type;
        
        public class BuiltInDataTypesClickHouseFixture : BuiltInDataTypesFixtureBase
        {
            protected override ITestStoreFactory TestStoreFactory => ClickHouseTestStoreFactory.Instance;

            public override bool StrictEquality => false;

            public override bool SupportsAnsi => true;

            public override bool SupportsUnicodeToAnsiConversion => false;

            public override bool SupportsLargeStringComparisons => false;

            public override bool SupportsBinaryKeys => false;

            public override bool SupportsDecimalComparisons => true;

            public override DateTime DefaultDateTime => DateTime.UnixEpoch;

            public int DecimalPrecision { get; set; } = 1;

            public int DoublePrecision { get; set; } = 8;

            public override bool PreservesDateTimeKind => false;

            public override DbContextOptionsBuilder AddOptions(DbContextOptionsBuilder builder)
            {
                return base.AddOptions(builder).LogTo(s => Trace.WriteLine(s));
            }
        }
    }
}
