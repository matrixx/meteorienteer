using UnityEngine;
using System.Collections.Generic;

public class TaivaanvahtiForm
{
	public List<TaivaanvahtiField> Fields {get; private set;}
	
	public void SetFields(List<TaivaanvahtiField> fields)
	{
		Fields = fields;
	}
}
