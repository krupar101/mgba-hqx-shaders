PYTHON ?= python3
CXX ?= c++
CXXFLAGS ?= -O2
VENDOR := vendor/vbam

.PHONY: generate reference test verify package preview clean

generate:
	$(PYTHON) tools/generate.py

reference: generate
	$(CXX) $(CXXFLAGS) -std=c++11 -shared -fPIC -I$(VENDOR) build/ref2.cpp -o build/ref2.so
	$(CXX) $(CXXFLAGS) -std=c++11 -shared -fPIC -I$(VENDOR) build/ref4.cpp -o build/ref4.so

test: reference
	$(PYTHON) -u tests/compare.py 2
	$(PYTHON) -u tests/compare.py 4
	$(PYTHON) -u tests/native_frame.py 2 240 160
	$(PYTHON) -u tests/native_frame.py 4 240 160

verify: test
	git diff --exit-code -- shaders

package:
	$(PYTHON) tools/package.py

preview: reference
	$(PYTHON) tools/preview.py

clean:
	rm -rf build dist
