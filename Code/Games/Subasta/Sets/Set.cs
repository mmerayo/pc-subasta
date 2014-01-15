namespace Games.Subasta.Sets
{
	class Set:ISet
	{
		public void Start()
		{
			throw new System.NotImplementedException();
		}

		public event SetEventHandler OnCompleted;
		protected virtual void FireCompleted()
		{
			if (OnCompleted != null)
			{
				OnCompleted(this);
			}
		}
	}
}
