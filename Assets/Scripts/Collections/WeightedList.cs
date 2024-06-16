using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeightedList<T> : IEnumerable<T> where T : IWeighted
{
	private readonly List<T> _items;
	private readonly System.Random _random;

	public WeightedList(List<T> items) : this( items, -1 ) { }

	public WeightedList(List<T> items, int seed)
	{
		_items = items;

		Debug.Log( "Using seed: " + seed );
		if ( seed < 0 )
		{
			_random = new System.Random();
		}
		else
		{
			_random = new System.Random( seed );
		}
	}

	public IEnumerator<T> GetEnumerator()
	{
		return _items.GetEnumerator();
	}

	// Based on https://docs.unity3d.com/Manual/class-Random.html
	public T Pick()
	{
		float total = 0;
		foreach ( var item in _items )
		{
			total += item.Weight;
		}

		float random = (float)_random.NextDouble() * total;

		foreach ( var item in _items )
		{
			if ( random < item.Weight )
			{
				return item;
			}

			random -= item.Weight;
		}

		return _items.Last();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)_items).GetEnumerator();
	}
}
