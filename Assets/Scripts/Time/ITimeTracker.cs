using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimeTracker {
	void clockUpdate(GameTimestamp timestamp);
}
