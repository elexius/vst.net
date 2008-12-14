#pragma once

// caches an array of T and grows on demand
template<class T>
ref class UnmanagedArray : System::IDisposable
{
public:
	UnmanagedArray(){ _array = NULL; _length = 0; }
	UnmanagedArray(T* instance, int length){ _array = instance; _length = length; }
	~UnmanagedArray(){ this->!UnmanagedArray(); }
	!UnmanagedArray(){ DeleteArray(); }

	T* GetArray(int length)
	{
		// check if length is ok
		if(_array != NULL &&
			_length < length)
		{
			DeleteArray();
		}

		if(_array == NULL)
		{
			AllocateArray(length);
		}

		return _array;
	}

	operator T*()
	{
		return _array;
	}

private:
	int _length;
	T* _array;

	void AllocateArray(int length)
	{
		_array = new T[length];
		_length = length;
	}

	void DeleteArray()
	{
		if(_array != NULL)
		{
			delete[] _array;
			_array = NULL;
			_length = 0;
		}
	}
};