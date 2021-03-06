﻿using System;
using System.Collections.Generic;
using ExpectedObjects.Specs.TestTypes;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace ExpectedObjects.Specs
{
	public class when_pushing_a_strategy
	{
		static TypeWithIEnumerable _actual;
		static Mock<IComparisonStrategy> _comparisonStrategyMock;
		static ExpectedObject _expected;

		static bool _result;

		Establish context = () =>
			{
				_comparisonStrategyMock = new Mock<IComparisonStrategy>();
				_comparisonStrategyMock.Setup(x => x.CanCompare(Moq.It.IsAny<Type>())).Returns(true);
				_comparisonStrategyMock.Setup(
					x => x.AreEqual(Moq.It.IsAny<object>(), Moq.It.IsAny<object>(), Moq.It.IsAny<IComparisonContext>()))
					.Returns(false);

				var expected = new TypeWithIEnumerable {Objects = new List<string> {"test string"}};
				_expected = new ExpectedObject(expected)
					.Configure(ctx => ctx.PushStrategy(_comparisonStrategyMock.Object));
				_actual = new TypeWithIEnumerable {Objects = new List<string> {"test string"}};
			};

		Because of = () => _result = _expected.Equals(_actual);

		It should_use_the_strategy = () => _result.ShouldBeFalse();
	}
}