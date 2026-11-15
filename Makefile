PYTHON ?= python3
CXX ?= c++
CXXFLAGS ?= -O2
VENDOR := vendor/vbam

.PHONY: generate generated-check repo-check reference test verify package package-test preview clean

generate:
	$(PYTHON) tools/generate.py

generated-check:
	$(PYTHON) tests/check_generated.py

repo-check:
	$(PYTHON) tests/check_repo.py
	$(PYTHON) tests/check_docs.py

reference: generate
	$(CXX) $(CXXFLAGS) -std=c++11 -shared -fPIC -I$(VENDOR) build/ref2.cpp -o build/ref2.so
	$(CXX) $(CXXFLAGS) -std=c++11 -shared -fPIC -I$(VENDOR) build/ref4.cpp -o build/ref4.so

test: repo-check reference
	$(PYTHON) -u tests/compare.py 2
	$(PYTHON) -u tests/compare.py 4
	$(PYTHON) -u tests/native_frame.py 2 240 160
	$(PYTHON) -u tests/native_frame.py 4 240 160

verify:
	$(MAKE) generated-check
	$(MAKE) test

package: generated-check repo-check
	$(PYTHON) tools/package.py

package-test: package
	$(PYTHON) tests/check_package.py

preview: reference
	$(PYTHON) tools/preview.py
	$(PYTHON) tests/check_preview.py

clean:
	rm -rf build dist
