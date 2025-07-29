using ic11.ControlFlow.Context;
using ic11.ControlFlow.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ic11.ControlFlow.TreeProcessing
{
    class MethodsRegisterRangesDistributor
    {
        private readonly FlowContext _flowContext;

        private readonly Dictionary<string, int> _methodCallCountMap = new();

        private readonly List<string> allRegisters = ["r0", "r1", "r2", "r3", "r4", "r5", "r6", "r7", "r8", "r9", "r10", "r11", "r12", "r13", "r14"];

        public MethodsRegisterRangesDistributor(FlowContext flowContext)
        {
            _flowContext = flowContext;
        }

        public void DoWork()
        {
            var main = _flowContext.DeclaredMethods["Main"];

            // Getting methods list
            CountCalls(main);

            var sortedMethods = _methodCallCountMap
                .OrderByDescending(x => x.Value * _flowContext.DeclaredMethods[x.Key].UsedRegistersCount)
                .Select(x => _flowContext.DeclaredMethods[x.Key])
                .ToList();

            sortedMethods.Insert(0, main);

            // Assigning registers
            foreach (var item in sortedMethods)
                AssignRegisters(item);

            // Shift registers to assigned sets
            foreach (var item in sortedMethods)
                ShiftRegisters(item);

            // Assigning pushes
            foreach (var item in sortedMethods)
                AssignPushForCalls(item);
        }

        private void AssignRegisters(MethodDeclaration method)
        {
            var registersUsageMap = allRegisters.ToDictionary(k => k, v => 0);
            GetRegistersUsageBeforeCalls(method, registersUsageMap);
            GetRegistersUsageAfterCalls(method, registersUsageMap);

            var leastUsedRegisters = registersUsageMap
                .OrderBy(x => x.Value)
                .ThenBy(x => int.Parse(x.Key[1..]))
                .Take(method.UsedRegistersCount)
                .Select(x => x.Key)
                .ToHashSet();

            method.AssignedRegisters = leastUsedRegisters;
        }

        void GetRegistersUsageBeforeCalls(MethodDeclaration method, Dictionary<string, int> map, HashSet<string> visitedMethods = null!)
        {
            if (visitedMethods is null)
                visitedMethods = new HashSet<string>();

            visitedMethods.Add(method.Name);

            foreach (var invoker in method.InvokedFrom)
            {
                foreach (var register in invoker.AssignedRegisters)
                    map[register] += 1;

                if (!visitedMethods.Contains(invoker.Name))
                    GetRegistersUsageBeforeCalls(invoker, map, visitedMethods);
            }
        }

        void GetRegistersUsageAfterCalls(MethodDeclaration method, Dictionary<string, int> map, HashSet<string> visitedMethods = null!)
        {
            if (visitedMethods is null)
                visitedMethods = new HashSet<string>();

            visitedMethods.Add(method.Name);

            foreach (var invokee in method.InvokedMethods)
            {
                foreach (var register in invokee.AssignedRegisters)
                    map[register] += 1;

                if (!visitedMethods.Contains(invokee.Name))
                    GetRegistersUsageAfterCalls(invokee, map, visitedMethods);
            }
        }

        private void CountCalls(MethodDeclaration currentMethod)
        {
            foreach (var item in currentMethod.InvokedMethods)
            {
                _methodCallCountMap.TryGetValue(item.Name, out var count);
                _methodCallCountMap[item.Name] = count + 1;

                if (count == 0)
                    CountCalls(item);
            }
        }

        private void ShiftRegisters(MethodDeclaration method)
        {
            var currentlyUsedRegisters = method.AllVariables
                .Where(v => v.Register is not null)
                .Where(v => v.LastReferencedIndex >= 0 || v.LastReassignedIndex >= 0 || v.IsParameter)
                .Select(v => v.Register)
                .Distinct()
                .ToList();

            var shiftMap = currentlyUsedRegisters.ToDictionary(k => k, v => (string)null!);
            var newRegisters = new HashSet<string>(method.AssignedRegisters);

            foreach (var k in shiftMap.Keys)
            {
                var availableRegister = newRegisters.First();
                shiftMap[k] = availableRegister;
                newRegisters.Remove(availableRegister);
            }

            foreach (var var in method.AllVariables.Where(v => v.LastReferencedIndex >= 0 || v.LastReassignedIndex >= 0 || v.IsParameter))
                var.Register = shiftMap[var.Register];
        }

        private void AssignPushForCalls(MethodDeclaration method)
        {
            foreach (var call in method.MethodCalls)
            {
                var inCodeUsedRegisters = call.Scope!.GetUsedRegisters(call.IndexInScope + 1, call.IndexInScope + 1);
                var inCalledMethodUsedRegisters = GetUsedRegistersDownstream(call);

                call.RegistersToPush = inCodeUsedRegisters
                    .Intersect(inCalledMethodUsedRegisters)
                    .ToHashSet();
            }
        }

        private HashSet<string> GetUsedRegistersDownstream(MethodCall call)
        {
            var registersUsageMap = allRegisters.ToDictionary(k => k, v => 0);
            GetRegistersUsageAfterCalls(call.Method!, registersUsageMap);
          
            return registersUsageMap
                .Where(p => p.Value > 0)
                .Select(p => p.Key)
                .Union(call.Method!.AssignedRegisters)
                .ToHashSet();
        }
    }
}
